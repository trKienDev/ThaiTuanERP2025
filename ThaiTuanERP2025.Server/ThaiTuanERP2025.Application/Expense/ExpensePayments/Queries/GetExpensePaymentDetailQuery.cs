using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Domain.Shared;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Queries
{
	public sealed record GetExpensePaymentDetailQuery(Guid Id) : IRequest<ExpensePaymentDetailDto?>;
	public sealed class GetExpensePaymentDetailQueryHandler : IRequestHandler<GetExpensePaymentDetailQuery, ExpensePaymentDetailDto?>
	{
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		public GetExpensePaymentDetailQueryHandler(IExpensePaymentReadRepository expensePaymentRepo)
		{
			_expensePaymentRepo = expensePaymentRepo;
		}

		public async Task<ExpensePaymentDetailDto?> Handle(GetExpensePaymentDetailQuery query, CancellationToken cancellationToken)
		{
			Guard.AgainstDefault(query.Id, nameof(query.Id));

			return await _expensePaymentRepo.GetDetailById(query.Id, cancellationToken);

		}

		public sealed class GetExpensePaymentDetailQueryValidator : AbstractValidator<GetExpensePaymentDetailQuery>
		{
			public GetExpensePaymentDetailQueryValidator()
			{
				RuleFor(x => x.Id).NotEmpty().WithMessage("Định danh của khoản thanh toán không được để trống");
			}
		}
	}
}
