using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.CreateOutgoingPayment;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetFollowingOutgoingPayments;

namespace ThaiTuanERP2025.Presentation.Controllers.Expense
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

		[HttpPost]
		public async Task<IActionResult> CreateOutgoingPayment([FromBody] OutgoingPaymentRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateOutgoingPaymentCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result, "Tạo khoản tiền ra thành công"));
		}

		[HttpGet("following")]
		public async Task<IActionResult> GetFollowingOutgoingPayments(CancellationToken cancellationToken)
		{
			var dtos = await _mediator.Send(new GetFollowingOutgoingPaymentsQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<OutgoingPaymentDto>>.Success(dtos));
		}
	}
}
