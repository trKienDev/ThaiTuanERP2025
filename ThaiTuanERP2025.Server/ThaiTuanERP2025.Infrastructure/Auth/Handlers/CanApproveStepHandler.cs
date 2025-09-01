using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Infrastructure.Auth.Requirements;

namespace ThaiTuanERP2025.Infrastructure.Auth.Handlers
{
	public sealed class CanApproveStepHandler : AuthorizationHandler<CanApproveStepRequirement, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CanApproveStepHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanApproveStepRequirement requirement, Guid stepInstanceId)
		{
			var userId = GetUserId(context.User);
			if (userId is null)
			{
				context.Fail();
				return;
			}

			var step = await _unitOfWork.ApprovalFlowInstances.GetStepWithFlowAsync(stepInstanceId);
			if (step is null)
			{
				context.Fail();
				return;
			}
			if(step.Status != ApprovalStepStatus.InProgress) {
				context.Fail();
				return;
			}

			var candidates = string.IsNullOrWhiteSpace(step.CandidatesJson)
				? new List<Guid>()
				: JsonSerializer.Deserialize<List<Guid>>(step.CandidatesJson) ?? new List<Guid>();

			if (candidates.Contains(userId.Value))
				context.Succeed(requirement);
			else
				context.Fail();
		}

		private static Guid? GetUserId(ClaimsPrincipal user) {
			var id = user.FindFirst("sub")?.Value
				?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Guid.TryParse(id, out var g) ? g : null;
		}
	}
}
