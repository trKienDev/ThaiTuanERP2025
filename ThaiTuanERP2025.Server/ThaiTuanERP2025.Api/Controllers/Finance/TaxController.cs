using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.CreateTax;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.DeleteTax;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.ToggleTaxStatus;
using ThaiTuanERP2025.Application.Finance.Commands.Taxes.UpdateTax;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.CheckPolicyNameAvailable;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetAllTaxes;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxById;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxByName;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxPaged;

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
		public async Task<ActionResult<ApiResponse<List<TaxDto>>>> GetAll([FromQuery] bool? isActive, [FromQuery] string? search, CancellationToken cancellationToken)
		{
			var listTaxes = await _mediator.Send(new GetAllTaxesQuery{ IsActive = isActive, Search = search }, cancellationToken);
			return Ok(ApiResponse<List<TaxDto>>.Success(listTaxes));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<ApiResponse<TaxDto>>> GetById(Guid id, CancellationToken cancellationToken) {
			var dto = await _mediator.Send(new GetTaxByIdQuery(id), cancellationToken);
			return Ok(ApiResponse<TaxDto>.Success(dto));
		}

		[HttpGet("by-name")]
		public async Task<ActionResult<ApiResponse<TaxDto?>>> GetByName([FromQuery] string policyName, CancellationToken cancellationToken) {
			var dto = await _mediator.Send(new GetTaxByNameQuery(policyName), cancellationToken);
			return Ok(ApiResponse<TaxDto?>.Success(dto));	
		}

		[HttpGet("check-available")]
		public async Task<ActionResult<ApiResponse<bool>>> CheckPolicyNameAvailable(
			[FromQuery] string policyName, [FromQuery] Guid? excludeId, CancellationToken cancellationToken
		) {
			var available = await _mediator.Send(new CheckPolicyNameAvailableQuery(policyName, excludeId), cancellationToken);
			return Ok(ApiResponse<bool>.Success(available));
		}

		[HttpGet("paged")]
		public async Task<ActionResult<ApiResponse<PagedResult<TaxDto>>>> GetPaged(
			[FromQuery] int pageIndex = 1,
			[FromQuery] int pageSize = 20,
			[FromQuery] string? keyword = null,
			[FromQuery] string? sort = null,
			[FromQuery] bool? isActive = null,
			CancellationToken ct = default
		) {
			var req = new PagedRequest
			{
				PageIndex = pageIndex,
				PageSize = pageSize,
				Keyword = keyword,
				Sort = sort,
				Filters = new Dictionary<string, string?> {
					{ "isActive", isActive?.ToString() }
				}
			};
			var result = await _mediator.Send(new GetTaxPagedQuery(req), ct);
			return Ok(ApiResponse<PagedResult<TaxDto>>.Success(result));
		}

		[HttpPost]
		public async Task<ActionResult<ApiResponse<TaxDto>>> Create([FromBody] CreateTaxCommand command, CancellationToken cancellationToken) {
			var dto = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<TaxDto>.Success(dto));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<ApiResponse<TaxDto>>> Update(Guid id, [FromBody] UpdateTaxCommand command, CancellationToken cancellationToken) {
			var dto = await _mediator.Send(command with { Id = id }, cancellationToken);
			return Ok(ApiResponse<TaxDto>.Success(dto));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult<ApiResponse<bool>>> ToggleStatus(Guid id, [FromQuery] bool isActive, CancellationToken cancellationToken) {
			var ok = await _mediator.Send(new ToggleTaxStatusCommand(id, isActive), cancellationToken);
			return Ok(ApiResponse<bool>.Success(ok));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken) {
			var ok = await _mediator.Send(new DeleteTaxCommand(id), cancellationToken);
			return Ok(ApiResponse<bool>.Success(ok));
		}
	}
}
