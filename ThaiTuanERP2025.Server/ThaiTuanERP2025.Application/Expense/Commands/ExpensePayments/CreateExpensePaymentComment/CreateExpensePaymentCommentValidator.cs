using FluentValidation;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePaymentComment
{
	public sealed class CreateExpensePaymentCommentValidator : AbstractValidator<CreateExpensePaymentCommentCommand>
	{
		public CreateExpensePaymentCommentValidator() {
			RuleFor(x => x.Request.ExpensePaymentId).NotEmpty();
			RuleFor(x => x.Request.Content)
			.NotEmpty().WithMessage("Content is required")
			.MaximumLength(2048);
			RuleForEach(x => x.Request.Attachments!).SetValidator(new AddAttachmentRequestValidator()!).When(x => x.Request.Attachments is not null);
		}

		private sealed class AddAttachmentRequestValidator : AbstractValidator<CommentAttachmentRequest>
		{
			public AddAttachmentRequestValidator()
			{
				RuleFor(x => x.FileName).NotEmpty().MaximumLength(256);
				RuleFor(x => x.FileUrl).NotEmpty().MaximumLength(512);
				RuleFor(x => x.MimeType).MaximumLength(128).When(x => x.MimeType is not null);
				RuleFor(x => x.FileSize).GreaterThanOrEqualTo(0);
			}
		}
	}
}
