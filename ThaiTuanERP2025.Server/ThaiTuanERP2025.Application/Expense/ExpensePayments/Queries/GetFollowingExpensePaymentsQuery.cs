using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Queries
{
	public sealed record GetFollowingExpensePaymentsQuery : IRequest<IReadOnlyList<ExpensePaymentLookupDto>>;

	public sealed class GetFollowingExpensePaymentsQueryHandler : IRequestHandler<GetFollowingExpensePaymentsQuery, IReadOnlyList<ExpensePaymentLookupDto>>
	{
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IUserReadRepostiory _userRepo;
		private readonly IFollowerReadRepository _followerRepo;
		public GetFollowingExpensePaymentsQueryHandler(
			IExpensePaymentReadRepository expensePaymentRepo, ICurrentUserService currentUser, IUserReadRepostiory userRepo,
			IFollowerReadRepository followerRepo
		) {
			_expensePaymentRepo = expensePaymentRepo;
			_currentUser = currentUser;
			_userRepo = userRepo;
			_followerRepo = followerRepo;
		}

		public async Task<IReadOnlyList<ExpensePaymentLookupDto>> Handle(GetFollowingExpensePaymentsQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new AppException("Không thể tìm thấy định danh user của bạn");
			var userExist = await _userRepo.ExistAsync(q => q.Id == userId && q.IsActive, cancellationToken);
			if (!userExist) throw new ValidationException("User của bạn không hợp lệ");

			var followingIds = await _followerRepo.ListProjectedAsync(
				q => q.Where(
					x => x.DocumentType == DocumentType.ExpensePayment
						&& x.UserId == userId
				).Select(x => x.DocumentId),
				cancellationToken: cancellationToken
			);

			var followingPayments = new List<ExpensePaymentLookupDto>();
			foreach(var id in followingIds)
			{
				var payment = await _expensePaymentRepo.GetLookupById(id, cancellationToken);
				if (payment != null)
				{
					followingPayments.Add(payment);
				}
			}

			return followingPayments;
		}
	}
}
