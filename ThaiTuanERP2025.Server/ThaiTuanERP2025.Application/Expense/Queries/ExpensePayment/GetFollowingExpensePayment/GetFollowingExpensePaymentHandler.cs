using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Followers.Enums;

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
			var expensePaymentIds = await _unitOfWork.Followers.ListAsync(
				q => q.Where(f => f.UserId == currentUserId && f.SubjectType == SubjectType.ExpensePayment).Select(f => f.SubjectId),
				cancellationToken: cancellationToken
			);
			if (expensePaymentIds.Count == 0)
				return Array.Empty<ExpensePaymentSummaryDto>();

			var idArr = expensePaymentIds.ToArray();
			var expensePayments = await _unitOfWork.ExpensePayments.FindIncludingAsync(
				p => idArr.Contains(p.Id),
				cancellationToken,
				p => p.CreatedByUser,
				p => p.CurrentWorkflowInstance!,
				p => p.CurrentWorkflowInstance!.Steps
			);

			var userIds = expensePayments.Where(p => p.CurrentWorkflowInstance != null)
				.SelectMany(p => p.CurrentWorkflowInstance!.Steps)
				.SelectMany(s => new[] { s.ApprovedBy, s.RejectedBy })
				.Where(id => id.HasValue)
				.Select(id => id!.Value)
				.Distinct().ToArray();

			var users = userIds.Length == 0 ? new List<Domain.Account.Entities.User>()
			    : await _unitOfWork.Users.ListAsync(q => q.Where(u => userIds.Contains(u.Id)),cancellationToken: cancellationToken);

			var userDtoDict = users.Select(u => _mapper.Map<UserDto>(u))
								.ToDictionary(u => u.Id, u => u);

			// Map ExpensePayment -> SummaryDto và truyền preload dict qua Items
			var dtos = expensePayments.Select(
				p => _mapper.Map<ExpensePaymentSummaryDto>(
					p, opt => { opt.Items["UserDict"] = userDtoDict; }
				)
			).ToArray();

			return dtos;
		}
	}
}
