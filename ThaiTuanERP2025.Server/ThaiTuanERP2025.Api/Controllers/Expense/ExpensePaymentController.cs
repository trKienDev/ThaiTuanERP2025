using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Commands;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;

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

               [HttpPost]
               public async Task<IActionResult> Create([FromBody] ExpensePaymentPayload payload, CancellationToken cancellationToken)
               {
                        var result = await _mediator.Send(new CreateExpensePaymentCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
               }
        }
}
