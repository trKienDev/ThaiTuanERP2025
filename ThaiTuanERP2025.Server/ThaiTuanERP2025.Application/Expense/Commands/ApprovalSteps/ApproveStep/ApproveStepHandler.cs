using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ApproveStep
{
	public sealed class ApproveStepHandler : IRequestHandler<ApproveStepCommand, ApprovalStepInstanceDto> {
		private readonly IUnitOfWork _unitOfWork;
		private ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public ApproveStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<ApprovalStepInstanceDto> Handle(ApproveStepCommand request, CancellationToken ct)
		{
			var userId = _currentUserService.GetUserIdOrThrow();
			var step = await _unitOfWork.ApprovalStepInstances.GetByIdWithWorkflowAsync(request.StepId)
				   ?? throw new InvalidOperationException("Không tìm thấy step.");
			StepGuards.EnsureWorkflowMatches(step, request.WorkflowId);
			StepGuards.EnsureWaiting(step);
			StepGuards.EnsureApproverAuthorized(step, userId);

			// Approve step
			step.Approve(userId, DateTime.UtcNow);
			if (!string.IsNullOrWhiteSpace(request.Body?.Comment))
			{
				var comment = (step.Comments ?? "") + $"\n[approve] {userId}: {request.Body.Comment}";
				step.GetType().GetProperty("Comments")!.SetValue(step, comment);
			}

			// Advance workflow
			var wf = step.WorkflowInstance;
			var nextOrder = step.Order + 1;
			var next = wf.Steps.OrderBy(s => s.Order).FirstOrDefault(s => s.Order == nextOrder);

			if (next is null)
			{
				wf.SetStatus(WorkflowStatus.Approved);
				wf.SetCurrentStep(null);
			}
			else
			{
				wf.SetCurrentStep(next.Order);
				next.Activate(DateTime.UtcNow);
			}

			await _unitOfWork.SaveChangesAsync(ct);
			return _mapper.Map<ApprovalStepInstanceDto>(step);
		}
	}
}
