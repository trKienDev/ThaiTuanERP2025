using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.SkipStep
{
	public sealed class SkipStepHandler : IRequestHandler<SkipStepCommand, ApprovalStepInstanceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public SkipStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<ApprovalStepInstanceDto> Handle(SkipStepCommand request, CancellationToken cancellationToken)
		{
			// (Tuỳ policy) chỉ Admin/Owner mới được skip
			if (!_currentUserService.IsInRole("Workflow.Admin"))
				throw new UnauthorizedAccessException("Bạn không có quyền bỏ qua bước.");

			var step = await _unitOfWork.ApprovalStepInstances.GetByIdWithWorkflowAsync(request.StepId, cancellationToken)
				   ?? throw new InvalidOperationException("Không tìm thấy step.");
			StepGuards.EnsureWorkflowMatches(step, request.WorkflowId);
			StepGuards.EnsureWaiting(step);

			step.Skip(request.Reason);

			// kích hoạt bước tiếp theo
			var wf = step.WorkflowInstance;
			var next = wf.Steps.OrderBy(s => s.Order).FirstOrDefault(s => s.Order == step.Order + 1);
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

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<ApprovalStepInstanceDto>(step);
		}
	}
}
