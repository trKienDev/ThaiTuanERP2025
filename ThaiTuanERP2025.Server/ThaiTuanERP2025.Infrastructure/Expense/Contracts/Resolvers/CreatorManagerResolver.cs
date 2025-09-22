using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Contracts.Resolvers;

namespace ThaiTuanERP2025.Infrastructure.Expense.Contracts.Resolvers
{
	public sealed class CreatorManagerResolver : IApproverResolver
	{
		public string Key => "creator-manager";
		private readonly IUnitOfWork _unitOfWork;	
		private readonly ICurrentUserService _currentUserService;
		public CreatorManagerResolver(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) {
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<(IReadOnlyList<Guid> Candidates, Guid? Default)> ResolveAsync(ResolverContext ctx, CancellationToken ct)
		{
			var userId = _currentUserService.GetUserIdOrThrow();

			// Lấy danh sách tất cả manager hợp lệ
			var managerIds = await _unitOfWork.Users.GetManagerIdsAsync(userId, ct);

			if (managerIds == null || managerIds.Count == 0)
				return (Array.Empty<Guid>(), null);

			// Theo logic repo: manager đầu tiên là IsPrimary, fallback đã được xử lý
			var defaultManager = managerIds.FirstOrDefault();

			return (managerIds, defaultManager);
		}

	}
}
