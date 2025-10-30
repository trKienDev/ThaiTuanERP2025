using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetGroups.CreateBudgetGroup;
using ThaiTuanERP2025.Application.Finance.Budgets.Requests;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/budget-group")]
	public class BudgetGroupController : Controller
	{
		private readonly IMediator _mediator;
		public BudgetGroupController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] BudgetGroupRequest request, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new CreateBudgetGroupCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
