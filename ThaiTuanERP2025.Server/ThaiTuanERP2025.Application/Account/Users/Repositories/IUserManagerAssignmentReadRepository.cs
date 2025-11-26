namespace ThaiTuanERP2025.Application.Account.Users.Repositories
{
	public interface IUserManagerAssignmentReadRepository
	{
		Task<List<Guid>> GetActiveManagerIdsAsync(Guid userId, CancellationToken cancellationToken = default);
	}
}
