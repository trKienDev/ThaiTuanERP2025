using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.CreateOutgoingPayment;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetFollowingOutgoingPayments;
using ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetOutgoingPaymentDetail;
using ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.ApproveOutgoingPayment;
using ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.MarkOutgoingPaymentCreated;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[Authorize]
	[ApiController]
	[Route("api/outgoing-payments")]
	public class OutgoingPaymentController : ControllerBase
	{
		private readonly IMediator _mediator;
		public OutgoingPaymentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("new")]
		public async Task<IActionResult> CreateOutgoingPayment([FromBody] OutgoingPaymentRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateOutgoingPaymentCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result, "Tạo khoản tiền ra thành công"));
		}

		[HttpGet("following")]
		public async Task<IActionResult> GetFollowingOutgoingPayments(CancellationToken cancellationToken)
		{
			var dtos = await _mediator.Send(new GetFollowingOutgoingPaymentsQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<OutgoingPaymentSummaryDto>>.Success(dtos));
		}

		[HttpGet("{id:guid}/detail")]
		public async Task<IActionResult> GetDetail(Guid id, CancellationToken cancellationToken)
		{
			var detailDto = await _mediator.Send(new GetOutgoingPaymentDetailQuery(id), cancellationToken);
			return Ok(ApiResponse<OutgoingPaymentDetailDto>.Success(detailDto));
		}

		[HttpPost("{id:guid}/approve")]
		public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new ApproveOutgoingPaymentCommand(id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("{id:guid}/created")]
		public async Task<IActionResult> MarkCreated(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new MarkOutgoingPaymentCreatedCommand(id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
