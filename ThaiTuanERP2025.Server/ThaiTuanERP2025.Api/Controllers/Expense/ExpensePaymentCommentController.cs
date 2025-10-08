using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePaymentComment;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.Expense.GetCommentsByExpensePaymentId;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Route("api/expense-payment-comments")]
	[Authorize]
	public sealed class ExpensePaymentCommentController : ControllerBase
	{
		private readonly IMediator _mediator;
		public ExpensePaymentCommentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Tạo bình luận cho một ExpensePayment (hỗ trợ đính kèm file, tag user, và reply 1 cấp).
		/// </summary>
		[HttpPost("{expensePaymentId:guid}")]
		[ProducesResponseType(typeof(ExpensePaymentCommentDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromRoute] Guid expensePaymentId, [FromBody] ExpensePaymentCommentRequest body, CancellationToken cancellationToken)
		{
			if (body is null) 
				return BadRequest(ApiResponse<object>.Fail("Body is required."));

			// Ép ExpensePaymentId từ route để đảm bảo an toàn (tránh client cố ý gửi khác)
			var request = body with { ExpensePaymentId = expensePaymentId };

			try {
				var result = await _mediator.Send(new CreateExpensePaymentCommentCommand(request), cancellationToken);

				return StatusCode(
					StatusCodes.Status201Created,
					ApiResponse<ExpensePaymentCommentDto>.Success(result, "Comment created successfully.")
				);
			} catch(Exception ex)
			{
				// log lỗi nếu cần
				return BadRequest(ApiResponse<object>.Fail($"Tạo comment thất bại: {ex.Message}"));
			}
		}

		/// <summary>
		/// (tuỳ chọn) Lấy toàn bộ comments theo paymentId (cha + replies 1 cấp).
		/// Nếu bạn đã có GetCommentsByExpensePaymentIdQuery thì map action này đến query đó.
		/// </summary>
		[HttpGet("by-payment/{expensePaymentId:guid}")]
		[ProducesResponseType(typeof(IReadOnlyList<ExpensePaymentCommentDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetByPaymentAsync([FromRoute] Guid expensePaymentId, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _mediator.Send(new GetCommentsByExpensePaymentIdQuery(expensePaymentId), cancellationToken);
				return Ok(ApiResponse<IReadOnlyList<ExpensePaymentCommentDto>>.Success(result, "Comments loaded successfully."));
			}
			catch (Exception ex)
			{
				return BadRequest(ApiResponse<object>.Fail("Failed to get comments", new[] { ex.Message }));
			}
		}
	}
}
