using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Queries
{
	public sealed record GetOutgoingPaymentDetailQuery(Guid OutgoingPaymentId) : IRequest<OutgoingPaymentDetailDto?>;
	public sealed class GetOutgoingPaymentDetailQueryHandler : IRequestHandler<GetOutgoingPaymentDetailQuery, OutgoingPaymentDetailDto?>
	{
		private readonly IOutgoingPaymentReadRepository _outgoingPaymentRepo;
		private readonly IFollowerReadRepository _followerRepo;
		private readonly IUserReadRepostiory _userRepo;
		public GetOutgoingPaymentDetailQueryHandler(
			IOutgoingPaymentReadRepository outgoingPaymentRepo, IFollowerReadRepository followerRepo, IUserReadRepostiory userRepo
		)
		{
			_outgoingPaymentRepo = outgoingPaymentRepo;
			_followerRepo = followerRepo;
			_userRepo = userRepo;	
		}

		public async Task<OutgoingPaymentDetailDto?> Handle(GetOutgoingPaymentDetailQuery query, CancellationToken cancellationToken)
		{
			var outgoingPaymentDetail = await _outgoingPaymentRepo.GetDetailById(query.OutgoingPaymentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy khoản chi");

			var followerIds = await _followerRepo.GetFollowerIdsByDocument(query.OutgoingPaymentId, DocumentType.OutgoingPayment, cancellationToken);
			if(followerIds.Any())
			{
				var followerBriefAvatar = await _userRepo.GetBriefWithAvatarManyAsync(followerIds, cancellationToken);
				outgoingPaymentDetail = outgoingPaymentDetail with
				{
					Followers = followerBriefAvatar,
				};
			}

			return outgoingPaymentDetail;
		}
	}

	public sealed class GetOutgoingPaymentDetailQueryValidator : AbstractValidator<GetOutgoingPaymentDetailQuery>
	{
		public GetOutgoingPaymentDetailQueryValidator()
		{
			RuleFor(x => x.OutgoingPaymentId).NotEmpty().WithMessage("Định danh khoản chi không được để trống");
		}
	}
}