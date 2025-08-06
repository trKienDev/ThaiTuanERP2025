using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.CreateBudgetGroup;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.DeleteBudgetGroup;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.UpdateBudgetGroup;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetGroups.GetAllBudgetGroups;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetGroups.GetBudgetGroupById;

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

		[HttpGet("all")]
		public async Task<ActionResult<ApiResponse<List<BudgetGroupDto>>>> GetAll() {
			var result = await _mediator.Send(new GetAllBudgetGroupsQuery());
			return Ok(ApiResponse<List<BudgetGroupDto>>.Success(result));
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid Id) {
			var budgetGroup = await _mediator.Send(new GetBudgetGroupByIdQuery(Id));
			return Ok(ApiResponse<BudgetGroupDto>.Success(budgetGroup));
		}

		[HttpPost]
		public async Task<ActionResult<ApiResponse<BudgetGroupDto>>> Create([FromBody] CreateBudgetGroupCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetGroupDto>.Success(result));
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<ApiResponse<BudgetGroupDto>>> Update(Guid id, [FromBody] UpdateBudgetGroupCommand command)
		{
			if (id != command.Id)
			{
				return BadRequest(ApiResponse<BudgetGroupDto>.Fail("Id không khớp với Id trong yêu cầu."));
			}
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetGroupDto>.Success(result));
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
		{
			await _mediator.Send(new DeleteBudgetGroupCommand(id));
			return Ok(ApiResponse<string>.Success("Xóa nhóm ngân sách thành công."));
		}
	}
}
