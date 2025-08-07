using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPeridos.CreateBudgetPeriod;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.DeleteBudgetPeriod;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.UpdateBudgetPeriod;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetPeriods.GetAllBudgetPeriods;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[Route("api/budget-period")]
	[ApiController]
	public class BudgetPeriodController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetPeriodController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var budgetPeriods = await _mediator.Send(new GetAllBudgetPeriodsQuery());
			return Ok(ApiResponse<List<BudgetPeriodDto>>.Success(budgetPeriods));
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateBudgetPeriodCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetPeriodDto>.Success(result));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBudgetPeriodCommand command)
		{
			if (id != command.Id)
			{
				return BadRequest(ApiResponse<string>.Fail("ID không khớp với ID trong yêu cầu"));
			}
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<string>.Success("Cập nhật kỳ ngân sách thành công"));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var command = new DeleteBudgetPeriodCommand(id);
			await _mediator.Send(command);
			return Ok(ApiResponse<string>.Success("Xóa kỳ ngân sách thành công"));
		}
	}
}
