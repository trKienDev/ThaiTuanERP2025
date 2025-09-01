using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Infrastructure.Auth.Requirements;

namespace ThaiTuanERP2025.Infrastructure.Auth.Handlers
{
	public sealed class CanCommentStepHandler : AuthorizationHandler<CanCommentStepRequirement, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CanCommentStepHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanCommentStepRequirement requirement, Guid StepInstanceId) {
			var userId = GetUserId(context.User); 
			if(userId is null) {
				context.Fail();
				return;
			}

			var step = await _unitOfWork.ApprovalFlowInstances.GetStepWithFlowAsync(StepInstanceId);
			if (step is null) {
				context.Fail();
				return;
			}

			var flow = step.FlowInstance;

			// requester của flow hoặc candidate của step
			var candidates = string.IsNullOrWhiteSpace(step.CandidatesJson)
				? new List<Guid>()
				: JsonSerializer.Deserialize<List<Guid>>(step.CandidatesJson) ?? new List<Guid>();
			var isCandidate = candidates.Contains(userId.Value);

			var isRequester = flow.CreatedByUserId == userId.Value;
			if (isRequester || isCandidate)
				context.Succeed(requirement);
			else 
				context.Fail();

		}

		private static Guid? GetUserId(ClaimsPrincipal user) {
			var id = user.FindFirst("sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return Guid.TryParse(id, out var g) ? g : null;
		}
	}
}
