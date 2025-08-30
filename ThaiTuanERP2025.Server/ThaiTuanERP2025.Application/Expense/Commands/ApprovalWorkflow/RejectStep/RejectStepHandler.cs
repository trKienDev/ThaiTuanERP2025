using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.RejectStep
{
	public sealed class RejectStepHandler : IRequestHandler<RejectStepCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;	
		public RejectStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(RejectStepCommand command, CancellationToken cancellationToken) {
			// 1) Load step + flow (tracking)
			var step = await _unitOfWork.ApprovalFlowInstances.GetStepWithFlowAsync(command.StepInstanceId, cancellationToken)
				?? throw new NotFoundException("Step not found");

			// 2) Chỉ cho reject khi step đang InProgress và chưa được chốt
			if (step.Status != ApprovalStepStatus.InProgress)
				throw new ConflictException("Step is not in progress");
			if (step.ApprovedByUserId is not null || step.ApprovedCount >= step.RequiredCount)
				throw new ConflictException("Step already approved");

			// 3) Concurrency check
			if (!command.RowVersion.SequenceEqual(step.RowVersion))
				throw new ConflictException("Step has been modified. Please refresh and try again");

			// 4) Authorization: step owner must be candidate
			var actor = _currentUserService.UserId
				?? throw new UnauthorizedException("Không xác định được user hiện tại");

			var candidates = JsonSerializer.Deserialize<List<Guid>>(step.CandidatesJson) ?? new();
			if (!candidates.Contains(actor))
				throw new ForbiddenException("Your are not a candidate approver of this step");

			// 5) Ghi action Reject
			var action = new ApprovalAction
			{
				Id = Guid.NewGuid(),
				StepInstanceId = step.Id,
				ActorUserId = actor,
				Action = ApprovalActionType.Reject,
				Comment = command.Reason,
				OccuredAt = DateTime.UtcNow,
			};

			await _unitOfWork.ApprovalActions.AddAsync(action);

			// 6) Cập nhật step & flow -> Reject
			step.Status = ApprovalStepStatus.Rejected;
			step.FinishedAt = DateTime.UtcNow;

			var flow = step.FlowInstance;
			flow.Status = ApprovalFlowStatus.Rejected;
			flow.FinishedAt = DateTime.UtcNow;

			// 7) Lưu
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// 8) (tùy chọn) Phát domain events để notify:
			// _domainEvents.Publish(new StepRejectedEvent(step.Id, actor));
			// _domainEvents.Publish(new FlowRejectedEvent(flow.Id));

			return Unit.Value;
		}
	}
}
