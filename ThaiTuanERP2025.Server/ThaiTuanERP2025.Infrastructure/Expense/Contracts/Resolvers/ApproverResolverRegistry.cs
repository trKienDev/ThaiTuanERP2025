using ThaiTuanERP2025.Application.Expense.Contracts.Resolvers;

namespace ThaiTuanERP2025.Infrastructure.Expense.Contracts.Resolvers
{
	public sealed class ApproverResolverRegistry : IApproverResolverRegistry
	{
		private readonly Dictionary<string, IApproverResolver> _map;
		public ApproverResolverRegistry(IEnumerable<IApproverResolver> resolvers)
		{
			_map = resolvers.ToDictionary(r => r.Key, StringComparer.OrdinalIgnoreCase);
		}

		public IApproverResolver? Get(string key)
		    => string.IsNullOrWhiteSpace(key) ? null : (_map.TryGetValue(key, out var r) ? r : null);
	}
}
