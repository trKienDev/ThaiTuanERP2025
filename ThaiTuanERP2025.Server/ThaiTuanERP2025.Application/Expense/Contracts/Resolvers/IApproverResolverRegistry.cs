namespace ThaiTuanERP2025.Application.Expense.Contracts.Resolvers
{
	public interface IApproverResolverRegistry
	{
		IApproverResolver? Get(string key);
	}
}
