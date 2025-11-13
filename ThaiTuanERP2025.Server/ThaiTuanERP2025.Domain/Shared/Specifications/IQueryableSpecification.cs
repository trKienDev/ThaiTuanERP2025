using System.Linq.Expressions;

namespace ThaiTuanERP2025.Domain.Shared.Specifications
{
	/// <summary>
	/// Specification hỗ trợ LINQ/EF Core: cung cấp biểu thức để Where().
	/// </summary>
	public interface IQueryableSpecification<T> : ISpecification<T>
	{
		Expression<Func<T, bool>> ToExpression();
	}
}
