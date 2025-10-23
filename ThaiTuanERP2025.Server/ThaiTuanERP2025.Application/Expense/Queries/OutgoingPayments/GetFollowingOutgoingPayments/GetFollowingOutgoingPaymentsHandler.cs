using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetFollowingOutgoingPayments
{
	public sealed class GetFollowingOutgoingPaymentsHandler : IRequestHandler<GetFollowingOutgoingPaymentsQuery, IReadOnlyCollection<OutgoingPaymentSummaryDto>>
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

		public async Task<IReadOnlyCollection<OutgoingPaymentSummaryDto>> Handle(GetFollowingOutgoingPaymentsQuery query, CancellationToken cancellationToken)
		{
			var currentUserId = _currentUserService.UserId;

			// 1) Lấy danh sách ID mà user đang follow (chủ đề OutgoingPayment)
			var idList = await _unitOfWork.Followers.ListAsync<Guid>(
			    q => q.Where(f => f.UserId == currentUserId && f.SubjectType == SubjectType.OutgoingPayment)
				  .Select(f => f.SubjectId),
			    cancellationToken: cancellationToken
			);

			if (idList.Count == 0) return Array.Empty<OutgoingPaymentSummaryDto>();

			// ListProjectedAsync<TDto> : EF Core chỉ select các cột cần thiết
			var summaries = await _unitOfWork.OutgoingPayments.ListProjectedAsync<OutgoingPaymentSummaryDto>(
				q => q.Where(p => idList.Contains(p.Id))
					.OrderByDescending(p => p.CreatedDate)
					.ProjectTo<OutgoingPaymentSummaryDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
			
			return summaries;
		}
	}
}
