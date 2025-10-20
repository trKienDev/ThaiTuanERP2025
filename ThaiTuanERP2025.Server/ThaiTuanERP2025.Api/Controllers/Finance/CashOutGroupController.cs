using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.CashoutOutGroup;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.CreateCashoutGroup;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.DeleteCashoutGroup;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.ToggleCashOutGroupActivate;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.CashoutGroups.GetAllCashoutGroups;
using ThaiTuanERP2025.Application.Finance.Queries.CashoutGroups.GetCashoutGroupById;

namespace ThaiTuanERP2025.Presentation.Controllers.Finance
{
	[ApiController]
	[Authorize]
	[Route("api/cashout-groups")]
	public class CashOutGroupController : ControllerBase
	{
		private readonly IMediator _mediator;
		public CashOutGroupController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<ActionResult> GetAll() {
			var data = await _mediator.Send(new GetAllCashoutGroupsQuery());
			return Ok(ApiResponse<List<CashoutGroupDto>>.Success(data));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult> GetById(Guid id) {
			var data = await _mediator.Send(new GetCashoutGroupByIdQuery(id));
			return Ok(ApiResponse<CashoutGroupDto>.Success(data));
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateCashoutGroupCommand command) {
			var dto = await _mediator.Send(command);
			return Ok(ApiResponse<CashoutGroupDto>.Success(dto));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCashoutGroupCommand command) {
			var result = await _mediator.Send(command with { Id = id});
			return Ok(ApiResponse<CashoutGroupDto>.Success(result));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult> ToggleStatus(Guid id, bool isActive) {
			var result = await _mediator.Send(new ToggleCashoutGroupActivateCommand(id, isActive));
			return Ok(ApiResponse<bool>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			var result = await _mediator.Send(new DeleteCashoutGroupCommand(id));
			return Ok(ApiResponse<bool>.Success(result));
		}
	}
}
