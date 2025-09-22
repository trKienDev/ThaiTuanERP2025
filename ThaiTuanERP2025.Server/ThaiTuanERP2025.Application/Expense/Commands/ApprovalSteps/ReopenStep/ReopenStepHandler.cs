using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ReopenStep
{
	public sealed class ReopenStepHandler : IRequestHandler<ReopenStepCommand, ApprovalStepInstanceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public ReopenStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<ApprovalStepInstanceDto> Handle(ReopenStepCommand request, CancellationToken cancellationToken)
		{
			var userId = _currentUserService.GetUserIdOrThrow();
			if (!_currentUserService.IsInRole("Workflow.Admin"))
				throw new UnauthorizedAccessException("Bạn không có quyền reopen bước.");

			var step = await _unitOfWork.ApprovalStepInstances.GetByIdWithWorkflowAsync(request.StepId, cancellationToken)
				   ?? throw new InvalidOperationException("Không tìm thấy step.");
			StepGuards.EnsureWorkflowMatches(step, request.WorkflowId);

			// chỉ reopen từ Approved/Rejected/Skipped/Expired
			if (step.Status is StepStatus.Pending or StepStatus.Waiting)
				throw new InvalidOperationException("Step đang mở; không cần reopen.");

			// reopen
			step.GetType().GetProperty("Status")!.SetValue(step, StepStatus.Waiting);
			step.GetType().GetProperty("StartedAt")!.SetValue(step, DateTime.UtcNow);
			step.GetType().GetProperty("DueAt")!.SetValue(step, DateTime.UtcNow.AddHours(step.SlaHours));
			var note = (step.Comments ?? "") + $"\n[reopen] by:{userId} reason:{request.Body?.Reason}";
			step.GetType().GetProperty("Comments")!.SetValue(step, note);

			// đưa workflow trở lại in-progress & currentStepOrder
			var wf = step.WorkflowInstance;
			wf.SetStatus(WorkflowStatus.InProgress);
			wf.SetCurrentStep(step.Order);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<ApprovalStepInstanceDto>(step);
		}
	}
}
