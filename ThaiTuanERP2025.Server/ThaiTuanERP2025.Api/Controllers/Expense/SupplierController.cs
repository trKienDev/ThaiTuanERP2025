using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Expense.Commands.Suppliers.CreateSupplier;
using ThaiTuanERP2025.Application.Expense.Commands.Suppliers.UpdateSupplier;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.Suppliers.CheckSupplierNameAvailable;
using ThaiTuanERP2025.Application.Expense.Queries.Suppliers.GetSupplierById;
using ThaiTuanERP2025.Application.Expense.Queries.Suppliers.GetSuppliers;

namespace ThaiTuanERP2025.Presentation.Controllers.Expense
{
	[Authorize]
	[ApiController]
	[Route("api/suppliers")]
	public class SupplierController : ControllerBase
	{
		private readonly IMediator _meidator;
		public SupplierController(IMediator mediator) => _meidator = mediator;

		[HttpGet("all")]
		public async Task<ActionResult<IReadOnlyList<SupplierDto>>> GetAll([FromQuery] string? keyword, CancellationToken cancellationToken) {
			var result = await _meidator.Send(new GetSuppliersQuery(keyword), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<SupplierDto>>.Success(result));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<SupplierDto>> GetById(Guid id, CancellationToken cancellationToken) {
			var result = await _meidator.Send(new GetSupplierByIdQuery(id), cancellationToken);
			return Ok(ApiResponse<SupplierDto>.Success(result));	
		}

		[HttpGet("check-available")]
		public async Task<ActionResult<ApiResponse<bool>>> CheckNameAvailable([FromQuery] string name, [FromQuery] Guid? excludeId, CancellationToken cancellationToken) {
			var available = await _meidator.Send(new CheckSupplierNameAvailableQuery(name, excludeId), cancellationToken);
			return Ok(ApiResponse<bool>.Success(available));
		}

		[HttpPost]
		public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierRequest body, CancellationToken cancellationToken) {
			var result = await _meidator.Send(new CreateSupplierCommand(body), cancellationToken);
			return Ok(ApiResponse<SupplierDto>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<SupplierDto>> Update(Guid id, [FromBody] UpdateSupplierRequest body, CancellationToken cancellationToken) {
			var result = await _meidator.Send(new UpdateSupplierCommand(id, body), cancellationToken);
			return Ok(ApiResponse<SupplierDto>.Success(result));
		}
	}
}
