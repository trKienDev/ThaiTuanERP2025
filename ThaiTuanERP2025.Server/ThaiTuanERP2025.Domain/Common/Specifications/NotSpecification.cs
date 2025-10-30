namespace ThaiTuanERP2025.Domain.Common.Specifications
{
	/// <summary>
	/// Đảo ngược kết quả của một Specification (NOT).
	/// </summary>
	public sealed class NotSpecification<T> : Specification<T>
	{
		private readonly Specification<T> _inner;

		public NotSpecification(Specification<T> inner)
		{
			_inner = inner;
		}

		public override bool IsSatisfiedBy(T entity) => !_inner.IsSatisfiedBy(entity);
	}
}
