using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.CreateTax;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.DeleteTax;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.ToggleTaxStatus;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.UpdateTax;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetAllTaxes;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[ApiController]
	[Authorize]
	[Route("api/taxes")]
	public class TaxController : ControllerBase
	{
		private readonly IMediator _mediator;
		public TaxController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll()
		{
			var listTaxes = await _mediator.Send(new GetAllTaxesQuery());
			return Ok(ApiResponse<List<TaxDto>>.Success(listTaxes));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult> GetById(Guid id) {
			var tax = await _mediator.Send(new GetTaxByIdQuery(id));
			return Ok(ApiResponse<TaxDto>.Success(tax));
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateTaxCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<TaxDto>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> Update(Guid id, [FromBody] UpdateTaxCommand command) {
			var result = await _mediator.Send(command with { Id = id });
			return Ok(ApiResponse<TaxDto>.Success(result));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult> ToggleStatus(Guid id, [FromBody] bool isActive) {
			var result = await _mediator.Send(new ToggleTaxStatusCommand(id, isActive));
			return Ok(ApiResponse<bool>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			var result = await _mediator.Send(new DeleteTaxCommand(id));
			return Ok(ApiResponse<bool>.Success(result));
		}
	}
}
