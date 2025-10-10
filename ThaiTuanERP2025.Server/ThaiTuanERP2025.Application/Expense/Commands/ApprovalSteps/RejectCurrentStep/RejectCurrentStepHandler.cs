using MediatR;
using System.Net.WebSockets;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectCurrentStep
{
	public sealed class RejectCurrentStepHandler : IRequestHandler<RejectCurrentStepCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public RejectCurrentStepHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(RejectCurrentStepCommand command, CancellationToken cancellationToken) {
			var workflowInstance = await _unitOfWork.ApprovalWorkflowInstances.SingleOrDefaultIncludingAsync(
				x => x.Id == command.WorkflowInstanceId,
				includes: x => x.Steps
			) ?? throw new NotFoundException("Không tìm thấy workflow instance");

			if(workflowInstance.Status is not WorkflowStatus.InProgress) 
				throw new ConflictException($"Workflow ở trạng thái {workflowInstance.Status}, không thể từ chối bước duyệt");

			var currentStep = workflowInstance.Steps.OrderBy(s => s.Order)
				.FirstOrDefault(s => s.Order == workflowInstance.CurrentStepOrder)
				?? workflowInstance.Steps.OrderBy(s => s.Order).FirstOrDefault();
			if (currentStep is null)
				throw new ConflictException("Workflow không có step hiện tại");
			if(currentStep.Id != command.StepInstanceId)
				throw new ConflictException("Chỉ được từ chối step đang hoạt động");

			// authorize
			var userId = command.UserId;
			var candidates = JsonUtils.ParseGuidArray(currentStep.ResolvedApproverCandidatesJson).ToHashSet();
			if (currentStep.FlowType == FlowType.Single) {
				if (currentStep.DefaultApproverId != userId)
					throw new ForbiddenException("Bạn không phải người duyệt được chỉ định ở bước này.");
			} else {
				if (!candidates.Contains(userId))
					throw new ForbiddenException("Bạn không có quyền duyệt ở bước này");
			}

			// Reject step
			var now = DateTime.UtcNow;
			currentStep.Reject(userId, command.Reason, now);

			// Finish workflow & payment
			workflowInstance.SetStatus(WorkflowStatus.Rejected);
			if(workflowInstance.DocumentType?.Equals("ExpensePayment", StringComparison.OrdinalIgnoreCase) == true) {
				var payment = await _unitOfWork.ExpensePayments.GetByIdAsync(workflowInstance.DocumentId);
				if (payment is not null)
					payment.Reject();
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
