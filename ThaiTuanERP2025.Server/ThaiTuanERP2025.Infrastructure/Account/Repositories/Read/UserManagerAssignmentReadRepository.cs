using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Read
{
	public sealed class UserManagerAssignmentReadRepository : IUserManagerAssignmentReadRepository
	{
		private readonly ThaiTuanERP2025DbContext _db;

		public UserManagerAssignmentReadRepository(ThaiTuanERP2025DbContext db)
		{
			_db = db;
		}

		public async Task<List<Guid>> GetActiveManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default)
		{
			return await _db.UserManagerAssignments.AsNoTracking()
				.Where(x => x.UserId == userId && x.RevokedAt == null)
				.OrderByDescending(x => x.IsPrimary)
				.ThenBy(x => x.AssignedAt)
				.Select(x => x.ManagerId)
				.ToListAsync(cancellationToken);
		}
	}
}
