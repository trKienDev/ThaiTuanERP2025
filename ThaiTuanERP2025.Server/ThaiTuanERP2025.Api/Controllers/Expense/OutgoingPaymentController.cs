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
	}
}
