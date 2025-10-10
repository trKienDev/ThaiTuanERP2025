using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	public sealed record StepOverrideRequest(int StepOrder, Guid? SelectedApproverId);

	/// <summary>
	/// Sinh ApprovalWorkflowInstance + ApprovalStepInstance cho 1 ExpensePayment ngay sau khi tạo.
	/// Mặc định tạo ở trạng thái Draft (chưa Start/Activate SLA).
	/// Trả về WorkflowInstanceId đã tạo. Nếu đã tồn tại AWI Draft/InProgress thì trả Guid.Empty (idempotent).
	/// </summary>
	/// 
	public sealed class ApprovalWorkflowService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ApprovalWorkflowResolverService _resolverService;
		private readonly INotificationService _notificationService;
		private readonly ITaskReminderService _taskReminderService;
		private readonly IApprovalStepService _approvalStepService;

		public ApprovalWorkflowService(
			IUnitOfWork unitOfWork, 
			ApprovalWorkflowResolverService resolverService, 
			INotificationService notificationService,
			ITaskReminderService taskReminderService,
			IApprovalStepService approvalStepService
		)
		{
			_unitOfWork = unitOfWork;
			_resolverService = resolverService;
			_notificationService = notificationService;
			_taskReminderService = taskReminderService;
			_approvalStepService = approvalStepService;
		}

		public async Task<Guid> CreateInstanceForExpensePaymentAsync(Guid paymentId, Guid workflowTemplateId, IReadOnlyCollection<StepOverrideRequest>? overrides, bool autoStart, bool linkToPayment, CancellationToken cancellationToken) {
			// 0) Chống trùng: đã có AWI Draft/InProgress cho document này?
			var existed = await _unitOfWork.ApprovalWorkflowInstances.AnyAsync(
				x => x.DocumentType == "ExpensePayment"
				&& x.DocumentId == paymentId
				&& (x.Status == WorkflowStatus.Draft || x.Status == WorkflowStatus.InProgress)
			);
			if (existed) return Guid.Empty;

			// 1) Load payment + template (+ steps)
			var payment = await _unitOfWork.ExpensePayments.SingleOrDefaultIncludingAsync(p => p.Id == paymentId)
				?? throw new NotFoundException("ExpensePayment not found");

			var tpl = await _unitOfWork.ApprovalWorkflowTemplates.SingleOrDefaultIncludingAsync(t => t.Id == workflowTemplateId && t.IsActive, includes: t => t.Steps)
				?? throw new ConflictException("Workflow template not found or inactive");
			if (tpl.Steps == null || !tpl.Steps.Any())
				throw new ConflictException("Workflow template has no steps");

			// 2 ) Tạo instance (Draft)
			var awi = new ApprovalWorkflowInstance(
				templateId: tpl.Id,
				templateVersion: tpl.Version,
				documentType: "ExpensePayment",
				documentId: payment.Id,
				createdByUserId: payment.CreatedByUserId,
				amount: payment.TotalWithTax,
				currency: null,
				budgetCode: null,
				costCenter: null,
				rawJson: null
			);
			await _unitOfWork.ApprovalWorkflowInstances.AddAsync(awi);

			var ovMap = overrides?.ToDictionary(x => x.StepOrder, x => x.SelectedApproverId) 
				?? new Dictionary<int, Guid?>();

			var firstOrder = tpl.Steps.Min(x => x.Order);
			// 3) Sinh step instances (Pending)
			foreach (var s in tpl.Steps.OrderBy(x => x.Order))
			{
				string? candidatesJson;
				Guid? defaultApprover = null;

				if (s.ApproverMode == ApproverMode.Standard)
				{
					candidatesJson = s.FixedApproverIdsJson;
					var arr = JsonUtils.ParseGuidArray(candidatesJson);
					defaultApprover = arr.FirstOrDefault();
				}
				else
				{
					var cands = await _resolverService.ResolveAsync(s.ResolverKey!, s.ResolverParamsJson, payment, cancellationToken);
					candidatesJson = JsonUtils.ToJsonArray(cands);
					defaultApprover = cands.FirstOrDefault();
				}

				ovMap.TryGetValue(s.Order, out var selected);

				var step = new ApprovalStepInstance(
					workflowInstanceId: awi.Id,
					templateStepId: s.Id,
					name: s.Name,
					order: s.Order,
					flowType: s.FlowType,
					slaHours: s.SlaHours,
					approverMode: s.ApproverMode,
					candidatesJson: candidatesJson,
					defaultApproverId: defaultApprover,
					selectedApproverId: selected,
					status: StepStatus.Pending
				);

				await _unitOfWork.ApprovalStepInstances.AddAsync(step);
			}

			// 4) Link nhanh về payment (nếu domain có property này)
			var linkMethod = payment.GetType()
				.GetMethod("LinkWorkflow",System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
			if (linkToPayment && linkMethod != null)
			{
				linkMethod.Invoke(payment, new object?[] { awi.Id });
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			await StartInstanceAsync(awi.Id, payment, CancellationToken.None);

			return awi.Id;
		}

		public async Task StartInstanceAsync(Guid instanceId, ExpensePayment payment,CancellationToken cancellationToken)
		{
			var workflowInstance = await LoadInstanceWithStepsAsync(instanceId, cancellationToken);
			if (workflowInstance.Status != WorkflowStatus.Draft) return;

			workflowInstance.MarkInProgress();

			var firstStep = workflowInstance.Steps.OrderBy(s => s.Order).First();
			await ActivateStepAsync(workflowInstance, firstStep, cancellationToken);

			// send notifcation + reminder
			await _approvalStepService.PublishAsync(workflowInstance, firstStep, payment.Name, payment.Id, "ExpensePayment", cancellationToken);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		private async Task<ApprovalWorkflowInstance> LoadInstanceWithStepsAsync(Guid instanceId, CancellationToken cancellationToken)
		{
			var ins = await _unitOfWork.ApprovalWorkflowInstances.SingleOrDefaultIncludingAsync(
				i => i.Id == instanceId,
				asNoTracking: false,
				cancellationToken: cancellationToken,
				i => i.Steps
			) ?? throw new NotFoundException($"Workflow instance {instanceId} not found");

			// Sắp xếp và dùng biến cục bộ thay vì gán ngược vào navigation
			var stepsOrdered = ins.Steps.OrderBy(s => s.Order).ToList();

			// Nếu các hàm sau cần steps theo thứ tự, hãy truyền stepsOrdered vào chúng,
			// hoặc dùng ngay stepsOrdered thay vì ins.Steps.
			// Ví dụ:
			// var first = stepsOrdered.First();

			return ins;
		}

		private async Task ActivateStepAsync(ApprovalWorkflowInstance ins, ApprovalStepInstance step, CancellationToken ct)
		{
			if (step.Status != StepStatus.Pending) return;

			// Resolve candidates nếu chưa có
			if (string.IsNullOrWhiteSpace(step.ResolvedApproverCandidatesJson))
			{
				var candidates = await ResolveApproversAsync(ins, step, ct);
				if (candidates.Count > 0)
					step.SetResolvedApproverCandidates(candidates);
			}

			// Activate (Waiting + DueAt)
			step.Activate(DateTime.UtcNow);

			// Nếu không có ai để notify/duyệt → Skip và nhảy bướcc

			// set current step
			ins.SetCurrentStep(step.Order);
		}

		private async Task<IReadOnlyCollection<Guid>> ResolveApproversAsync(ApprovalWorkflowInstance ins, ApprovalStepInstance step, CancellationToken cancellationToken)
		{
			// 0) Nếu đã resolve sẵn thì dùng luôn
			if (!string.IsNullOrWhiteSpace(step.ResolvedApproverCandidatesJson))
			{
				try
				{
					var ids = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(step.ResolvedApproverCandidatesJson) ?? new();
					return ids;
				}
				catch { /* ignore */ }
			}

			// 1) Load template step
			ApprovalStepTemplate? tpl = null;
			if (step.TemplateStepId.HasValue)
			{
				tpl = await _unitOfWork.ApprovalStepTemplates
				    .SingleOrDefaultIncludingAsync(t => t.Id == step.TemplateStepId.Value, cancellationToken: cancellationToken);
			}

			// Fallback: khớp bằng (WorkflowTemplateId + Order) nếu TemplateStepId không có
			if (tpl is null)
			{
				tpl = await _unitOfWork.ApprovalStepTemplates
					.SingleOrDefaultIncludingAsync(t =>
						t.WorkflowTemplateId == ins.TemplateId && t.Order == step.Order,
						cancellationToken: cancellationToken
					);
			}

			if (tpl is null) return Array.Empty<Guid>();

			// 2) Theo mode
			if (tpl.ApproverMode == ApproverMode.Standard)
			{
				// Nếu template khai báo cố định
				if (!string.IsNullOrWhiteSpace(tpl.FixedApproverIdsJson))
				{
					try
					{
						var ids = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(tpl.FixedApproverIdsJson!) ?? new();
						return ids;
					}
					catch { return Array.Empty<Guid>(); }
				}

				// Không có danh sách cố định -> thử DefaultApproverId của step
				if (step.DefaultApproverId.HasValue && step.DefaultApproverId.Value != Guid.Empty)
					return new[] { step.DefaultApproverId.Value };

				return Array.Empty<Guid>();
			}
			else // ApproverMode.Condition
			{
				// Lấy context document nếu cần (ExpensePayment)
				ExpensePayment? payment = null;
				if (ins.DocumentType == "ExpensePayment")
				{
					payment = await _unitOfWork.ExpensePayments
					    .SingleOrDefaultIncludingAsync(p => p.Id == ins.DocumentId, cancellationToken: cancellationToken);
				}

				if (payment is null)
					throw new InvalidOperationException("Resolver requires ExpensePayment context");

				var cands = await _resolverService.ResolveAsync(
					tpl.ResolverKey!,
					tpl.ResolverParamsJson, 
					payment, 
					cancellationToken
				);

				return cands?.ToArray() ?? Array.Empty<Guid>();
			}
		}

		// Trong ApprovalWorkflowService.cs (cùng lớp với MoveToNextStepAsync)
		private ApprovalStepInstance? GetNextStep(ApprovalWorkflowInstance ins, ApprovalStepInstance current)
		{
			// Sắp xếp bước theo Order
			var ordered = ins.Steps.OrderBy(s => s.Order).ToList();

			// Tìm vị trí bước hiện tại
			var idx = ordered.FindIndex(s => s.Id == current.Id);
			if (idx < 0 || idx >= ordered.Count - 1)
				return null;

			// Trả về bước kế tiếp còn Pending (tuỳ enum của bạn, đổi lại nếu khác)
			for (int i = idx + 1; i < ordered.Count; i++)
			{
				var s = ordered[i];
				if (s.Status == StepStatus.Pending)   // hoặc Created/New nếu bạn đặt tên khác
					return s;
			}
			return null;
		}


	}
}
