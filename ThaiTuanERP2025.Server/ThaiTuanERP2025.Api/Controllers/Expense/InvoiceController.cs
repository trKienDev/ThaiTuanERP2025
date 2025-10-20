using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceFollowers;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceLine;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.AttachInvoiceFile;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.CreateInvoiceDraft;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceFile;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceFollower;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceLine;
using ThaiTuanERP2025.Application.Expense.Commands.Invoices.ReplaceMainInvoiceFile;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetInvoiceById;
using ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetInvoicesPaged;
using ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetMyInvoices;

namespace ThaiTuanERP2025.Presentation.Controllers.Expense
{
	[Authorize]
	[ApiController]
	[Route("api/invoices")]
	public class InvoiceController : ControllerBase
	{
		private readonly IMediator _mediator;
		public InvoiceController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("draft")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> CreateDraft([FromBody] CreateInvoiceRequest request)
		{
			var result = await _mediator.Send(new CreateInvoiceCommand(request));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> GetById(Guid id) {
			var result = await _mediator.Send(new GetInvoiceByIdQuery(id));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpGet]
		public async Task<ActionResult<ApiResponse<PagedResult<InvoiceDto>>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? keyword = null) {
			var result = await _mediator.Send(new GetInvoicesPagedQuery(page, pageSize, keyword));
			return Ok(ApiResponse<PagedResult<InvoiceDto>>.Success(result));
		}

		[Authorize]
		[HttpGet("mine")]
		public async Task<ActionResult<PagedResult<InvoiceDto>>> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new GetMyInvoicesQuery(page, pageSize), cancellationToken);
			return Ok(ApiResponse<PagedResult<InvoiceDto>>.Success(result));
		}

		[HttpPost("{id}/lines")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> AddLine(Guid id, [FromBody] AddInvoiceLineRequest body) {
			var request = body with { InvoiceId = id };
			var result = await _mediator.Send(new AddInvoiceLineCommand(request));	
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpDelete("{id}/lines/{lineId}")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> RemoveLine(Guid id, Guid lineId)
		{
			var result = await _mediator.Send(new RemoveInvoiceLineCommand(id, lineId));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpPost("{id}/files")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> AttachFile(Guid id, [FromBody] AttachInvoiceFileRequest body) {
			var request = body with { InvoiceId = id };
			var result = await _mediator.Send(new AttachInvoiceFileCommand(request));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpPost("{id}/files/replace-main")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> ReplaceMain(Guid id, [FromBody] ReplaceMainInvocieFileRequest body) {
			var request = body with { InvoiceId = id };
			var result = await _mediator.Send(new ReplaceMainInvoiceFileCommand(request));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpDelete("{id}/files/{fileId}")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> RemoveFile(Guid id, Guid fileId)
		{
			var result = await _mediator.Send(new RemoveInvoiceFileCommand(id, fileId));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpPost("{id}/followers")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> AddFollower(Guid id, [FromBody] Guid userId) {
			var result = await _mediator.Send(new AddInvoiceFollowersCommand(id, userId));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}

		[HttpDelete("{id}/followers/{userId}")]
		public async Task<ActionResult<ApiResponse<InvoiceDto>>> RemoveFollower(Guid id, Guid userId)
		{
			var result = await _mediator.Send(new RemoveInvoiceFollowerCommand(id, userId));
			return Ok(ApiResponse<InvoiceDto>.Success(result));
		}
	}
}
