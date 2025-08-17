using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Partner.Commands.Suppliers.CreateSupplier;
using ThaiTuanERP2025.Application.Partner.Commands.Suppliers.DeleteSupplier;
using ThaiTuanERP2025.Application.Partner.Commands.Suppliers.ToggleSupplierStatus;
using ThaiTuanERP2025.Application.Partner.Commands.Suppliers.UpdateSupplier;
using ThaiTuanERP2025.Application.Partner.Queries.Suppliers.GetAllSuppliers;
using ThaiTuanERP2025.Application.Partner.Queries.Suppliers.GetSupplierByCode;
using ThaiTuanERP2025.Application.Partner.Queries.Suppliers.GetSupplierById;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Api.Controllers.Partner
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class SupplierController : ControllerBase
	{
		private readonly IMediator _mediator;
		public SupplierController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateSupplierCommand(request), cancellationToken);
			return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<SupplierDto>.Success(result));
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
			return Ok(ApiResponse<SupplierDto>.Success(result));
		}

		[HttpGet("by-code/{code}")]
		public async Task<IActionResult> GetByCode([FromRoute] string code, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetSupplierByCodeQuery(code), cancellationToken);
			if (result is null) return NotFound(ApiResponse<string>.Fail($"Mã nhà cung cấp: ${code} không tìm thấy"));
			return Ok(ApiResponse<SupplierDto>.Success(result));
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(
			[FromQuery] string? keyword, [FromQuery] bool? isActive,
			[FromQuery] string? currency, [FromQuery] int page = 1, 
			[FromQuery] int pageSize = 20, CancellationToken ct = default)
		{
			var result = await _mediator.Send(new GetAllSuppliersQuery(keyword, isActive, currency, page, pageSize), ct);
			return Ok(ApiResponse<PagedResult<SupplierDto>>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update(
			[FromRoute] Guid id, [FromBody] UpdateSupplierRequest request, CancellationToken cancellationToken)
		{
			if (id == Guid.Empty) return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));
			if (request is null) return BadRequest(ApiResponse<string>.Fail("Dữ liệu cập nhật không hợp lệ"));
			try
			{
				var result = await _mediator.Send(new UpdateSupplierCommand(id, request), cancellationToken);
				return Ok(ApiResponse<SupplierDto>.Success(result));
			}
			catch (NotFoundException ex)
			{
				return NotFound(ApiResponse<string>.Fail(ex.Message));
			}
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<IActionResult> ToggleStatus(
			[FromRoute] Guid id, [FromQuery] bool isActive, CancellationToken cancellationToken)
		{
			if (id == Guid.Empty) return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));
			try
			{
				var result = await _mediator.Send(new ToggleSupplierStatusCommand(id, isActive), cancellationToken);
				return Ok(ApiResponse<SupplierDto>.Success(result));
			}
			catch (NotFoundException ex)
			{
				return NotFound(ApiResponse<string>.Fail(ex.Message));
			}
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			if (id == Guid.Empty) return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));
			try
			{
				await _mediator.Send(new DeleteSupplierCommand	(id), cancellationToken);
				return NoContent();
			}
			catch (NotFoundException ex)
			{
				return NotFound(ApiResponse<string>.Fail(ex.Message));
			}
		}
	}
}
