using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Queries
{
	public sealed record GetExpensePaymentDetailQuery(Guid Id) : IRequest<ExpensePaymentDetailDto?>;
	public sealed class GetExpensePaymentDetailQueryHandler : IRequestHandler<GetExpensePaymentDetailQuery, ExpensePaymentDetailDto?>
	{
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		private readonly IFollowerReadRepository _folllowerRepo;
		private readonly IUserReadRepostiory _userRepo;
		public GetExpensePaymentDetailQueryHandler(IExpensePaymentReadRepository expensePaymentRepo, IFollowerReadRepository followerRepo, IUserReadRepostiory userRepo)
		{
			_expensePaymentRepo = expensePaymentRepo;
			_folllowerRepo = followerRepo;
			_userRepo = userRepo;
		}

		public async Task<ExpensePaymentDetailDto?> Handle(GetExpensePaymentDetailQuery query, CancellationToken cancellationToken)
		{
			var paymentDetail = await _expensePaymentRepo.GetDetailById(query.Id, cancellationToken)
				?? throw new NotFoundException("Khoản thanh toán không tồn tại");

			var followerIds = await _folllowerRepo.GetFollowerIdsByDocument(paymentDetail.Id, DocumentType.ExpensePayment, cancellationToken);
			
			if(followerIds.Any())
			{
				var followerBriefAvatar = await _userRepo.GetBriefWithAvatarManyAsync(followerIds, cancellationToken);

				paymentDetail = paymentDetail with
				{
					Followers = followerBriefAvatar
				};
			}

			return paymentDetail;
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
