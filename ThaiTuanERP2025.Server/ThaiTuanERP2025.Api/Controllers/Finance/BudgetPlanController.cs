using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.CreateBudgetPlan;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.DeleteBudgetPlan;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.UpdateBudgetPlan;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetPlans.GetAllBudgetPlans;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/budget-plan")]
	public class BudgetPlanController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetPlanController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllBudgetPlansQuery());
			return Ok(ApiResponse<List<BudgetPlanDto>>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateBudgetPlanCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetPlanDto>.Success(result));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, UpdateBudgetPlanCommand command)
		{
			if (id != command.Id)
			{
				return BadRequest(ApiResponse<BudgetPlanDto>.Fail("Id Kế hoạch ngân sách không hợp lệ"));
			}
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetPlanDto>.Success(result));
		}

		[HttpDelete("{id}")]	
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new DeleteBudgetPlanCommand { Id = id });
			return Ok(ApiResponse<Unit>.Success(Unit.Value));
		}
	}
}
