using ThaiTuanERP2025.Application.Account.Users.Services;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Account.Services
{
	public sealed class UserManagerService : IUserManagerService
	{
		private readonly IUnitOfWork _uow;
		public UserManagerService(IUnitOfWork uow)
		{
			_uow = uow;
		}

		/// <summary>
		/// Replace toàn bộ manager: remove old, add new, promote primary.
		/// </summary>
		public async Task ReplaceAsync(Guid userId, IReadOnlyList<Guid> managerIds, Guid primaryManagerId, CancellationToken cancellationToken)
		{
			// Load user
			var userExist = await _uow.Users.ExistAsync(q => q.Id == userId, cancellationToken: cancellationToken);
			if (!userExist) throw new NotFoundException($"Không tìm thấy user {userId}");

			// Chuẩn hoá input
			var desired = managerIds.Distinct().Where(x => x != userId).ToList();

			// Load assignments
			var currentAssignments = await _uow.Users.GetActiveManagerAssignmentsAsync(userId, cancellationToken);
			var currentIds = currentAssignments.Select(a => a.ManagerId).ToHashSet();

			// 1) Revoke những manager không còn cần
			foreach (var a in currentAssignments.Where(a => !desired.Contains(a.ManagerId)))
				a.Revoke();

			// 2) Add managers mới
			var newAssignments = desired
				.Where(id => !currentIds.Contains(id))
				.Select(id => new UserManagerAssignment(userId, id, false))
				.ToList();

			if (newAssignments.Any())
				await _uow.Users.AddAssignmentsAsync(newAssignments);

			// 3) Set primary
			if (primaryManagerId != Guid.Empty && desired.Contains(primaryManagerId))
			{
				// demote tất cả
				foreach (var a in currentAssignments.Where(a => a.IsActive))
					a.DemoteFromPrimary();

				foreach (var a in newAssignments)
					a.DemoteFromPrimary();

				// promote primary
				var primaryAssign = currentAssignments.FirstOrDefault(a => a.IsActive && a.ManagerId == primaryManagerId)
					?? newAssignments.FirstOrDefault(a => a.ManagerId == primaryManagerId);

				primaryAssign?.PromoteToPrimary();
			}
		}

		/// <summary>
		/// Merge mode: thêm manager mới vào list hiện tại, không xoá manager cũ.
		/// </summary>
		public async Task MergeAsync(Guid userId, IReadOnlyList<Guid> newManagerIds, Guid primaryManagerId, CancellationToken cancellationToken)
		{
			// Load current managers
			var currentAssignments = await _uow.Users.GetActiveManagerAssignmentsAsync(userId, cancellationToken);
			var currentIds = currentAssignments.Select(a => a.ManagerId).ToHashSet();

			// Merge
			var merged = currentIds.Union(newManagerIds).Where(x => x != userId).ToList();

			// Replace using replace logic but without revoke
			await ReplaceAsync(userId, merged, primaryManagerId, cancellationToken);
		}
	}
}
