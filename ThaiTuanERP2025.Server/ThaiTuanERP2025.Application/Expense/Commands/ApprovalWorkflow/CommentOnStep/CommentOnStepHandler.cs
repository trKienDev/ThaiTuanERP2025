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

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.CommentOnStep
{
	public sealed class CommentOnStepHandler : IRequestHandler<CommentOnStepCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;	
		public CommentOnStepHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}	

		public async Task<Unit> Handle(CommentOnStepCommand command, CancellationToken cancellationToken) {
			// 1) Actor
			var actor = _currentUserService.UserId 
				?? throw new UnauthorizedException("Không xác định được user hiện tại");

			// 2) Load step + flow (tracking)
			var step = await _unitOfWork.ApprovalFlowInstances.GetStepWithFlowAsync(command.StepInstanceId, cancellationToken)
				?? throw new NotFoundException("Step not found");
			
			var flow = step.FlowInstance;

			// 3) Authorization: cho phép requester của flow hoặc candidate của step được comment
			var candidates = JsonSerializer.Deserialize<List<Guid>>(step.CandidatesJson) ?? new();
			var isRequester = flow.CreatedByUserId == actor;
			var isCandidate = candidates.Contains(actor);

			if (!isRequester && !isCandidate)
				throw new ForbiddenException("Bạn không có quyền bình luận ở bước này");

			// 4) Validation nooij dung (ít nhất phải có text hoặc có file)
			var hasText = !string.IsNullOrWhiteSpace(command.Comment);
			var hasFiles = command.AttachmentFileIds is { Count: > 0 };
			if (!hasText && !hasFiles)
				throw new ValidationException("Comment", "Vui lòng nhập nội dung hoặc đính kèm tệp");

			// 5) Ghi action Comment
			var action = new ApprovalAction
			{
				Id = Guid.NewGuid(),
				StepInstanceId = step.Id,
				ActorUserId = actor,
				Comment = command.Comment,
				AttachmentFileIdsJson = hasFiles ? JsonSerializer.Serialize(command.AttachmentFileIds) : null,
				OccuredAt = DateTime.UtcNow,
			};

			await _unitOfWork.ApprovalActions.AddAsync(action);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
