using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Commands;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
        [ApiController]
        [Route("api/outgoing-payment")]
        [Authorize]
        public sealed class OutgoingPaymentController : ControllerBase
        {
                private readonly IMediator _mediator;
                public OutgoingPaymentController(IMediator mediator)
                {
                        _mediator = mediator;
		}

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new GetAllOutgoingPaymentsQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<OutgoingPaymentDto>>.Success(result));
                }

                [HttpGet("{id:guid}/detail")]
                public async Task<IActionResult> GetDetail([FromRoute] Guid id, CancellationToken cancellationToken)
                {
                        var detail = await _mediator.Send(new GetOutgoingPaymentDetailQuery(id), cancellationToken);
                        if (detail is null)
                                return BadRequest(ApiResponse<string>.Fail("Không tìm thấy chi tiết khoản chi"));

                        return Ok(ApiResponse<OutgoingPaymentDetailDto>.Success(detail));
                }

                [HttpGet("following")]
                public async Task<IActionResult> GetFollowing(CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new GetFollowingOutgoingPaymentsQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<OutgoingPaymentLookupDto>>.Success(result));
                }

                [HttpPost]
                public async Task<IActionResult> Create([FromBody] OutgoingPaymentPayload payload, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new CreateOutgoingPaymentCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }

                [HttpPost("{id:guid}/approve")]
                public async Task<IActionResult> Approve([FromRoute] Guid id, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new ApproveOutgoingPaymentCommand(id), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
	}
}
