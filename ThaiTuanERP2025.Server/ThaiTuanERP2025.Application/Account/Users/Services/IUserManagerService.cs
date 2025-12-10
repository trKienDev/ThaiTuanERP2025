namespace ThaiTuanERP2025.Application.Account.Users.Services
{
	public interface IUserManagerService
	{
		Task ReplaceAsync(Guid userId, IReadOnlyList<Guid> managerIds, Guid primaryManagerId, CancellationToken cancellationToken);
		Task MergeAsync(Guid userId, IReadOnlyList<Guid> newManagerIds, Guid primaryManagerId, CancellationToken cancellationToken);
	}
}
