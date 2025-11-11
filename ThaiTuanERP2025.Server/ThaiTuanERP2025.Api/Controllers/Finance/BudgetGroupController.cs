using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.BudgetGroups;
using ThaiTuanERP2025.Application.Finance.BudgetGroups.Commands;
using ThaiTuanERP2025.Application.Finance.BudgetGroups.Query;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/budget-group")]
	public class BudgetGroupController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetGroupController(IMediator mediator) => _mediator = mediator;
		
		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetAllBudgetGroupsQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetGroupDto>>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Handle([FromBody] CreateBudgetGroupCommand command, CancellationToken cancellationToken) {
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
 	}
}
