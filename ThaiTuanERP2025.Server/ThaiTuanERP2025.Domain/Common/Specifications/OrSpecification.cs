namespace ThaiTuanERP2025.Domain.Common.Specifications
{
	/// <summary>
	/// Kết hợp hai Specification bằng toán tử logic OR.
	/// </summary>
	public sealed class OrSpecification<T> : Specification<T>
	{
		private readonly Specification<T> _left;
		private readonly Specification<T> _right;

		public OrSpecification(Specification<T> left, Specification<T> right)
		{
			_left = left;
			_right = right;
		}

		public override bool IsSatisfiedBy(T entity) => _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
	}
}
