using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.CreateCashOutGroup;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.DeleteCashOutGroup;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.ToggleCashOutGroupStatus;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.UpdateCashOutGroup;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.CashOutGroups.GetAllCashOutGroups;
using ThaiTuanERP2025.Application.Finance.Queries.CashOutGroups.GetCashOutGroupById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
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

		[HttpGet]
		public async Task<ActionResult> GetAll() {
			var data = await _mediator.Send(new GetAllCashOutGroupsQuery());
			return Ok(ApiResponse<List<CashOutGroupDto>>.Success(data));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult> GetById(Guid id) {
			var data = await _mediator.Send(new GetCashOutGroupByIdQuery(id));
			return Ok(ApiResponse<CashOutGroupDto>.Success(data));
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateCashOutGroupCommand command) {
			var dto = await _mediator.Send(command);
			return Ok(ApiResponse<CashOutGroupDto>.Success(dto));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCashOutGroupCommand command) {
			var result = await _mediator.Send(command with { Id = id});
			return Ok(ApiResponse<CashOutGroupDto>.Success(result));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult> ToggleStatus(Guid id, bool isActive) {
			var result = await _mediator.Send(new ToggleCashOutGroupStatusCommand(id, isActive));
			return Ok(ApiResponse<bool>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			var result = await _mediator.Send(new DeleteCashOutGroupCommand(id));
			return Ok(ApiResponse<bool>.Success(result));
		}
	}
}
