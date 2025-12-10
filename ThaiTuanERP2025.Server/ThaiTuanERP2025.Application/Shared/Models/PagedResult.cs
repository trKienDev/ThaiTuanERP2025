namespace ThaiTuanERP2025.Application.Shared.Models
{
	public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
}
