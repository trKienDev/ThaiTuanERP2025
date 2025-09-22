using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.OverrideApprover
{
	public sealed class OverrideApproverHandler : IRequestHandler<OverrideApproverCommand, ApprovalStepInstanceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public OverrideApproverHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<ApprovalStepInstanceDto> Handle(OverrideApproverCommand request, CancellationToken ct)
		{
			var userId = _currentUserService.GetUserIdOrThrow();
			// (Tuỳ policy) người lập chứng từ hoặc Admin mới được override khi AllowOverride=true
			var step = await _unitOfWork.ApprovalStepInstances.GetByIdWithWorkflowAsync(request.StepId, ct)
				   ?? throw new InvalidOperationException("Không tìm thấy step.");
			StepGuards.EnsureWorkflowMatches(step, request.WorkflowId);
			if (step.Status != StepStatus.Pending && step.Status != StepStatus.Waiting)
				throw new InvalidOperationException("Chỉ override khi step chưa/đang chờ duyệt.");

			// Kiểm tra policy AllowOverride (lưu ở TemplateStep, bạn có thể copy sang instance tại snapshot)
			// Ở đây minh hoạ: cho phép override nếu step.ApproverMode==Condition
			if (step.ApproverMode != ApproverMode.Condition)
				throw new InvalidOperationException("Step không cho phép override approver.");

			// new approver phải thuộc candidates
			var candidates = JsonGuidArray.Parse(step.ResolvedApproverCandidatesJson);
			if (candidates.Length > 0 && !candidates.Contains(request.Body.NewApproverId))
				throw new InvalidOperationException("Người được chọn không nằm trong danh sách hợp lệ.");

			step.GetType().GetProperty("SelectedApproverId")!.SetValue(step, request.Body.NewApproverId);

			// ghi history
			var note = (step.Comments ?? "") + $"\n[override] by:{userId} to:{request.Body.NewApproverId} reason:{request.Body.Reason}";
			step.GetType().GetProperty("Comments")!.SetValue(step, note);

			await _unitOfWork.SaveChangesAsync(ct);
			return _mapper.Map<ApprovalStepInstanceDto>(step);
		}
	}
}
