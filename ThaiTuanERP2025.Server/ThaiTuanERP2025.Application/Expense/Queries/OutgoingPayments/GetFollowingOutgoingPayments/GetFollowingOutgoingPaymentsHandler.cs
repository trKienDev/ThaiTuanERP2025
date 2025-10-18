using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetFollowingOutgoingPayments
{
	public sealed class GetFollowingOutgoingPaymentsHandler : IRequestHandler<GetFollowingOutgoingPaymentsQuery, IReadOnlyCollection<OutgoingPaymentDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public GetFollowingOutgoingPaymentsHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<IReadOnlyCollection<OutgoingPaymentDto>> Handle(GetFollowingOutgoingPaymentsQuery query, CancellationToken cancellationToken)
		{
			var currentUserId = _currentUserService.UserId;
			var outgoingPaymentIds = await _unitOfWork.Followers.ListAsync(
				q => q.Where(f => f.UserId == currentUserId && f.SubjectType == SubjectType.OutgoingPayment)
					  .Select(f => f.SubjectId),
				cancellationToken: cancellationToken
			);
			if(outgoingPaymentIds.Count == 0)
				return Array.Empty<OutgoingPaymentDto>();

			var idArr = outgoingPaymentIds.ToArray();
			var outgoingPayments = await _unitOfWork.OutgoingPayments.FindIncludingAsync(
				p => idArr.Contains(p.Id),
				cancellationToken
			);

			var dtos = outgoingPayments.Select(p => _mapper.Map<OutgoingPaymentDto>(p)).ToArray();
			return dtos;
		}
	}
}
