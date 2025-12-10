using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Queries
{
	public sealed record GetFollowingOutgoingPaymentsQuery : IRequest<IReadOnlyList<OutgoingPaymentLookupDto>>;
	public sealed class GetFollowingOutgoingPaymentsQueryHandler : IRequestHandler<GetFollowingOutgoingPaymentsQuery, IReadOnlyList<OutgoingPaymentLookupDto>>
	{
		private readonly IOutgoingPaymentReadRepository _outgoingPaymentRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IUserReadRepostiory _userRepo;
		private readonly IFollowerReadRepository _followerRepo;
		private readonly IMapper _mapper;
		public GetFollowingOutgoingPaymentsQueryHandler(
			ICurrentUserService currentUser, IOutgoingPaymentReadRepository outgoingPaymentRepo, IUserReadRepostiory userRepo,
			IFollowerReadRepository followerRepo, IMapper mapper
		)
		{
			_outgoingPaymentRepo = outgoingPaymentRepo;
			_currentUser = currentUser;	
			_userRepo = userRepo;
			_followerRepo = followerRepo;	
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<OutgoingPaymentLookupDto>> Handle(GetFollowingOutgoingPaymentsQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new ValidationException("User không hợp lệ");
			var userExist = await _userRepo.ExistAsync(q => q.Id == userId, cancellationToken);
			if(!userExist) throw new ValidationException("User không hợp lệ");

			var followingOutgoingPaymentIds = await _followerRepo.GetFollowingDocumentIdsByType(userId, DocumentType.OutgoingPayment, cancellationToken);
			
			if(!followingOutgoingPaymentIds.Any()) 
				return Array.Empty<OutgoingPaymentLookupDto>();

			return await _outgoingPaymentRepo.ListProjectedAsync(
				q => q.Where(x => followingOutgoingPaymentIds.Contains(x.Id))
					.ProjectTo<OutgoingPaymentLookupDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);

		}
	}
}
