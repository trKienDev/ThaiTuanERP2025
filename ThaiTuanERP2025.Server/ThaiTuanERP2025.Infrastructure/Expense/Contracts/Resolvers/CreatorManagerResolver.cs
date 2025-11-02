using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Contracts.Resolvers;

namespace ThaiTuanERP2025.Infrastructure.Expense.Contracts.Resolvers
{
	public sealed class CreatorManagerResolver : IApproverResolver
	{
		public string Key => "creator-manager";
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserReadRepostiory _userReadRepo;
		private readonly ICurrentUserService _currentUserService;
		public CreatorManagerResolver(
			IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUserReadRepostiory userReadRepo
		) {
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_userReadRepo = userReadRepo;
		}

		public async Task<(IReadOnlyList<Guid> Candidates, Guid? Default)> ResolveAsync(ResolverContext ctx, CancellationToken cancellationToken)
		{
			var userId = _currentUserService.GetUserIdOrThrow();

			// Lấy danh sách tất cả manager hợp lệ

			var managerIds = await _userReadRepo.GetManagerIdsAsync(userId, cancellationToken);

			if (managerIds == null || managerIds.Count == 0)
				return (Array.Empty<Guid>(), null);

			// Theo logic repo: manager đầu tiên là IsPrimary, fallback đã được xử lý
			var defaultManager = managerIds.FirstOrDefault();

			return (managerIds, defaultManager);
		}

	}
}
