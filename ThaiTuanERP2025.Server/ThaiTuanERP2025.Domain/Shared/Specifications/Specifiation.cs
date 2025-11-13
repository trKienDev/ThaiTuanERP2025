namespace ThaiTuanERP2025.Domain.Shared.Specifications
{
	/// <summary>
	/// Lớp cơ sở trừu tượng giúp kết hợp nhiều Specification lại với nhau
	/// (AND, OR, NOT) để mô tả điều kiện nghiệp vụ phức tạp.
	/// </summary>
	public abstract class Specification<T> : ISpecification<T>
	{
		public abstract bool IsSatisfiedBy(T entity);

		public Specification<T> And(Specification<T> other)
		    => new AndSpecification<T>(this, other);

		public Specification<T> Or(Specification<T> other)
		    => new OrSpecification<T>(this, other);

		public Specification<T> Not()
		    => new NotSpecification<T>(this);
	}
}
