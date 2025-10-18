using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePayments;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetExpensePaymentDetail;
using ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetFollowingExpensePayment;

namespace ThaiTuanERP2025.Presentation.Controllers.Expense
{
	[ApiController]
	[Route("api/expense-payments")]
	[Authorize]
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
			return Ok(ApiResponse<Guid>.Success(result));
		}

		[HttpGet("{id:guid}/detail")]
		[ProducesResponseType(typeof(ApiResponse<ExpensePaymentDetailDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetDetail(Guid id, CancellationToken cancellationToken)
		{
			var dto = await _mediator.Send(new GetExpensePaymentDetailQuery(id), cancellationToken);
			return Ok(ApiResponse<ExpensePaymentDetailDto>.Success(dto));
		}

		[HttpGet("following")]
		public async Task<IActionResult> GetFollowingExpensePayments(CancellationToken cancellationToken)
		{
			var dtos = await _mediator.Send(new GetFollowingExpensePaymentQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<ExpensePaymentSummaryDto>>.Success(dtos));
		}
	}
}
