using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePayments;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Route("api/expense-payments")]
	public class ExpensePaymentController : ControllerBase
	{
		private readonly IMediator _mediator;
		public ExpensePaymentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> CreateExpensePayment([FromBody] ExpensePaymentRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateExpensePaymentCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
