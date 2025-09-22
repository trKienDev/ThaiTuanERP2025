using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectStep
{
	public sealed class RejectStepHandler : IRequestHandler<RejectStepCommand, ApprovalStepInstanceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public RejectStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<ApprovalStepInstanceDto> Handle(RejectStepCommand request, CancellationToken cancellationToken)
		{
			var userId = _currentUserService.GetUserIdOrThrow();
			var step = await _unitOfWork.ApprovalStepInstances.GetByIdWithWorkflowAsync(request.StepId, cancellationToken)
				?? throw new InvalidOperationException("Không tìm thấy step.");
			StepGuards.EnsureWorkflowMatches(step, request.WorkflowId);
			StepGuards.EnsureWaiting(step);
			StepGuards.EnsureApproverAuthorized(step, userId);

			step.Reject(userId, request.Body?.Reason, DateTime.UtcNow);

			// Policy mặc định: reject → cả workflow bị Rejected
			step.WorkflowInstance.SetStatus(WorkflowStatus.Rejected);
			step.WorkflowInstance.SetCurrentStep(null);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<ApprovalStepInstanceDto>(step);
		}
	}
}
