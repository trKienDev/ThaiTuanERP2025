using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Common.Utils;
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
		public ApprovalWorkflowService(IUnitOfWork unitOfWork, ApprovalWorkflowResolverService resolverService)
		{
			_unitOfWork = unitOfWork;
			_resolverService = resolverService;
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
			ApprovalStepInstance? firstStepRef = null;
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

			// 5) AutoStart (không khuyến nghị ngay khi tạo)
			if (autoStart)
			{
				awi.Start(firstOrder);
				firstStepRef?.Activate(DateTime.UtcNow);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return awi.Id;
		}
	}
}
