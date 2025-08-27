using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Partner.Commands.PartnerBankAccounts.DeletePartnerBankAccountBySupplierId;
using ThaiTuanERP2025.Application.Partner.Commands.PartnerBankAccounts.UpsertPartnerBankAccountForSupplier;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Application.Partner.Queries.PartnerBankAccounts.GetPartnerBankAccountBySupplierId;

namespace ThaiTuanERP2025.Api.Controllers.Partner
{
	[ApiController]
	[Route("api/partners/suppliers/{supplierId:guid}/bank-account")]
	public class SupplierBankAccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		public SupplierBankAccountController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromRoute] Guid supplierId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetPartnerBankAccountBySupplierIdQuery(supplierId));
			return Ok(ApiResponse<PartnerBankAccountDto>.Success(result));
		}

		[HttpPut]
		public async Task<IActionResult> Upsert([FromRoute] Guid supplierId, [FromBody] UpsertPartnerBankAccountRequest request, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpsertPartnerBankAccountForSupplierCommand(supplierId, request), cancellationToken);
			return Ok(ApiResponse<PartnerBankAccountDto>.Success(result));
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromRoute] Guid supplierId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new DeletePartnerBankAccountBySupplierIdCommand(supplierId), cancellationToken);
			return Ok(ApiResponse<bool>.Success(result));
		}
	}
}
