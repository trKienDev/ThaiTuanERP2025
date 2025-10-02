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
		public ApprovalWorkflowService(IUnitOfWork unitOfWork, ApprovalWorkflowResolverService resolverService, INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_resolverService = resolverService;
			_notificationService = notificationService;
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

			await StartInstanceAsync(awi.Id, CancellationToken.None);

			return awi.Id;
		}

		public async Task StartInstanceAsync(Guid instanceId, CancellationToken ct)
		{
			var ins = await LoadInstanceWithStepsAsync(instanceId, ct);
			if (ins.Status != WorkflowStatus.Draft) return;

			ins.MarkInProgress();

			var first = ins.Steps.OrderBy(s => s.Order).First();
			await ActivateStepAsync(ins, first, ct);

			// >>> GỬI THÔNG BÁO BƯỚC ĐẦU
			var targetUserIds = await ResolveTargetsForNotificationAsync(first, ct);
			await _notificationService.NotifyStepActivatedAsync(ins, first, targetUserIds, ct);

			await _unitOfWork.SaveChangesAsync(ct);
		}

		private async Task MoveToNextStepAsync(ApprovalWorkflowInstance ins, ApprovalStepInstance current, CancellationToken ct)
		{
			current.MarkApproved(); // hoặc Approved/Rejected tuỳ rule
			var next = GetNextStep(ins, current);

			if (next is null)
			{
				ins.MarkInProgress();
				return;
			}

			await ActivateStepAsync(ins, next, ct);

			// >>> GỬI THÔNG BÁO BƯỚC KẾ
			var targetUserIds = await ResolveTargetsForNotificationAsync(next, ct);
			await _notificationService.NotifyStepActivatedAsync(ins, next, targetUserIds, ct);
		}

		private Task<IReadOnlyCollection<Guid>> ResolveTargetsForNotificationAsync(ApprovalStepInstance step, CancellationToken ct)
		{
			// Nếu đã có SelectedApproverId (flow single + override/chọn sẵn) => chỉ 1 người
			if (step.SelectedApproverId.HasValue && step.SelectedApproverId.Value != Guid.Empty)
				return Task.FromResult<IReadOnlyCollection<Guid>>(new[] { step.SelectedApproverId.Value });

			Guid[] ids = Array.Empty<Guid>();
			if (!string.IsNullOrWhiteSpace(step.ResolvedApproverCandidatesJson))
			{
				try
				{
					ids = System.Text.Json.JsonSerializer
					    .Deserialize<List<Guid>>(step.ResolvedApproverCandidatesJson!)?
					    .ToArray() ?? Array.Empty<Guid>();
				}
				catch { /* ignore */ }
			}

			if (ids.Length == 0 && step.DefaultApproverId.HasValue && step.DefaultApproverId.Value != Guid.Empty)
				return Task.FromResult<IReadOnlyCollection<Guid>>(new[] { step.DefaultApproverId.Value });

			return Task.FromResult<IReadOnlyCollection<Guid>>(ids);
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

			// Nếu không có ai để notify/duyệt → Skip và nhảy bước
			var targets = await ResolveTargetsForNotificationAsync(step, ct);
			if (targets.Count == 0 && !step.SelectedApproverId.HasValue && !step.DefaultApproverId.HasValue)
			{
				step.Skip("No approver candidates");
				await MoveToNextStepAsync(ins, step, ct);
			}
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
