namespace ThaiTuanERP2025.Application.Expense.Contracts.Resolvers
{
	public interface IApproverResolver
	{
		/// Key duy nhất (map với TemplateStep.ResolverKey), ví dụ: "creator-manager"
		string Key { get; }

		/// Trả về danh sách ứng viên (Candidates) và người mặc định (Default) nếu có
		Task<(IReadOnlyList<Guid> Candidates, Guid? Default)> ResolveAsync(ResolverContext ctx, CancellationToken ct);
	}
}
