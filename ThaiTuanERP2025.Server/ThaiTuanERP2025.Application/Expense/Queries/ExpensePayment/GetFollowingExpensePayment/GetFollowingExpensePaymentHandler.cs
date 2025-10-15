using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetFollowingExpensePayment
{
	public sealed class GetFollowingExpensePaymentHandler : IRequestHandler<GetFollowingExpensePaymentQuery, IReadOnlyCollection<ExpensePaymentSummaryDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public GetFollowingExpensePaymentHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<IReadOnlyCollection<ExpensePaymentSummaryDto>> Handle(GetFollowingExpensePaymentQuery query, CancellationToken cancellationToken)
		{
			var currentUserId = _currentUserService.UserId;
			var expensePaymentIds = await _unitOfWork.ExpensePaymentFollowers.ListAsync(
				q => q.Where(f => f.UserId == currentUserId).Select(f => f.ExpensePaymentId),
				cancellationToken: cancellationToken
			);

			var expensePayments = await _unitOfWork.ExpensePayments.FindIncludingAsync(
				    p => expensePaymentIds.Contains(p.Id),
				    cancellationToken,
				    p => p.CreatedByUser
			);

			var dtos = expensePayments.Select(p => _mapper.Map<ExpensePaymentSummaryDto>(p))
				.ToArray();

			return dtos;
		}
	}
}
