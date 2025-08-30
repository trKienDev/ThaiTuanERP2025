using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetCurrentUser;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.ApproveStep
{
	public sealed class ApproveStepHandler : IRequestHandler<ApproveStepCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public ApproveStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(ApproveStepCommand command, CancellationToken cancellationToken) {
			// 1) Load StepInstance + FlowInstance
			var stepInstance = await _unitOfWork.ApprovalFlowInstances.GetStepWithFlowAsync(command.StepInstanceId, cancellationToken);
			if (stepInstance == null)
				throw new InvalidOperationException("Không tìm thấy step");

			// 2) Kiểm tra trạng thái step
			if (stepInstance.Status != ApprovalStepStatus.InProgress)
				throw new InvalidOperationException("Step is not in progess");
			if (stepInstance.ApprovedCount >= stepInstance.RequiredCount || stepInstance.ApprovedByUserId is not null)
				throw new InvalidOperationException("Step already approved");

			// 3) Kiểm tra currency
			if (!command.RowVersion.SequenceEqual(stepInstance.RowVersion))
				throw new ConflictException("Step has benn modified. Please refresh and try again.");

			// 4) Kiểm tra quyền (actor ∈ Candidates)
			var actor = _currentUserService.UserId
				?? throw new UnauthorizedException("Không xác định được user hiện tại");
			var candidates = JsonSerializer.Deserialize<List<Guid>>(stepInstance.CandidatesJson) ?? new();
			if (!candidates.Contains(actor))
				throw new ForbiddenException("You are not a candidate approver of this step.");

			// 5) Ghi action Approve
			var action = new ApprovalAction
			{
				Id = Guid.NewGuid(),
				StepInstanceId = stepInstance.Id,
				ActorUserId = actor,
				Action = ApprovalActionType.Approve,
				Comment = command.Comment,
				OccuredAt = DateTime.UtcNow,
				CreatedByUserId = actor
			};

			// tuỳ BaseRepository: nếu không có repo riêng cho Action,
			// bạn có thể thêm phương thức AddAction vào ApprovalFlowInstanceRepository
			await _unitOfWork.ApprovalActions.AddAsync(action);

			// 6) Đóng step hiện tại
			stepInstance.ApprovedByUserId = actor;
			stepInstance.ApprovedCount = 1; // RequiredCount hiện tại luôn = 1 theo yêu cầu
			stepInstance.Status = ApprovalStepStatus.Approved;
			stepInstance.FinishedAt = DateTime.UtcNow;

			// 7) Mở step kế tiếp (nếu có)
			var flowInstance = stepInstance.FlowInstance;

			var nextStep = await _unitOfWork.ApprovalFlowInstances.GetNextPendingStepAsync(flowInstance.Id, stepInstance.OrderIndex, cancellationToken);
			if(nextStep is not null) {
				nextStep.Status = ApprovalStepStatus.InProgress;
				nextStep.StartedAt = DateTime.UtcNow;
				// nếu bạn đã copy SlaHours vào StepInstance khi submit thì set DeadlineAt ở đây:
				// next.DeadlineAt = next.StartedAt.Value.AddHours(next.SlaHours);
			} else {
				// Không còn step → hoàn tất flow
				flowInstance.Status = ApprovalFlowStatus.Approved;
				flowInstance.FinishedAt = DateTime.UtcNow;
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// 8) (Tuỳ chọn) phát domain events → Notification
			// - StepApprovedEvent(step.Id, actor)
			// - Nếu flow hoàn tất: FlowCompletedEvent(flow.Id)

			return Unit.Value;
		}
	}
}
