namespace ThaiTuanERP2025.Domain.Common.Specifications
{
	/// <summary>
	/// Interface đại diện cho một Specification pattern cơ bản trong Domain layer.
	/// Dùng để kiểm tra liệu một entity có thỏa mãn điều kiện nghiệp vụ nào đó hay không.
	/// </summary>
	public interface ISpecification<T>
	{
		bool IsSatisfiedBy(T entity);
	}
}
