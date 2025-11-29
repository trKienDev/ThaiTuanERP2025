using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Commands;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
        [ApiController]
        [Authorize]
        [Route("api/expense-payment")]
        public class ExpensePaymentController : ControllerBase
        {
                private readonly IMediator _mediator;
                public ExpensePaymentController(IMediator mediator)
                {
                        _mediator = mediator;
                }

                [HttpGet("following")]
                public async Task<IActionResult> GetFollowing(CancellationToken cancellationToken) {
                        var followingDtos = await _mediator.Send(new GetFollowingExpensePaymentsQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<ExpensePaymentLookupDto>>.Success(followingDtos));
                }

                [HttpGet("detail/{id:guid}")]
                public async Task<IActionResult> GetDetail([FromRoute] Guid id, CancellationToken cancellationToken)
                {
                        var detail = await _mediator.Send(new GetExpensePaymentDetailQuery(id), cancellationToken);
                        if (detail is null)
                                return BadRequest(ApiResponse<string>.Fail("Không tìm thấy chi tiết thanh toán"));

                        return Ok(ApiResponse<ExpensePaymentDetailDto>.Success(detail));
                }

               [HttpPost]
               public async Task<IActionResult> Create([FromBody] ExpensePaymentPayload payload, CancellationToken cancellationToken)
               {
                        var result = await _mediator.Send(new CreateExpensePaymentCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
               }
        }
}
