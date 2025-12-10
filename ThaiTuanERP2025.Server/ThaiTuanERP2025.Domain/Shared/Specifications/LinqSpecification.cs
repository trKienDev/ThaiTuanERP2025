using System.Linq.Expressions;

namespace ThaiTuanERP2025.Domain.Shared.Specifications
{
	/// <summary>
	/// Base cho specification có thể dùng với LINQ/EF.
	/// Kế thừa Specification<T> (in-memory) và triển khai IQueryableSpecification<T>.
	/// </summary>
	public abstract class LinqSpecification<T> : Specification<T>, IQueryableSpecification<T>
	{
		public abstract Expression<Func<T, bool>> ToExpression();

		// Reuse cho in-memory check:
		public override bool IsSatisfiedBy(T entity) => ToExpression().Compile().Invoke(entity);

		/// <summary>Áp spec vào một IQueryable.</summary>
		public IQueryable<T> Apply(IQueryable<T> queryable) => queryable.Where(ToExpression());

		/// <summary>AND hai spec (kết hợp biểu thức an toàn cho EF).</summary>
		public LinqSpecification<T> And(LinqSpecification<T> other) => new CombinedLinqSpecification<T>(this, other, ExpressionExtensions.AndAlso);

		/// <summary>OR hai spec.</summary>
		public LinqSpecification<T> Or(LinqSpecification<T> other) => new CombinedLinqSpecification<T>(this, other, ExpressionExtensions.OrElse);

		/// <summary>NOT spec.</summary>
		public new LinqSpecification<T> Not() => new NotLinqSpecification<T>(this);

		private sealed class CombinedLinqSpecification<TX> : LinqSpecification<TX>
		{
			private readonly LinqSpecification<TX> _left;
			private readonly LinqSpecification<TX> _right;
			private readonly Func<Expression<Func<TX, bool>>, Expression<Func<TX, bool>>, Expression<Func<TX, bool>>> _combiner;

			public CombinedLinqSpecification(
				LinqSpecification<TX> left,
				LinqSpecification<TX> right,
				Func<Expression<Func<TX, bool>>, Expression<Func<TX, bool>>, Expression<Func<TX, bool>>> combiner
			) {
				_left = left;
				_right = right;
				_combiner = combiner;
			}

			public override Expression<Func<TX, bool>> ToExpression() => _combiner(_left.ToExpression(), _right.ToExpression());
		}

		private sealed class NotLinqSpecification<TX> : LinqSpecification<TX>
		{
			private readonly LinqSpecification<TX> _inner;
			public NotLinqSpecification(LinqSpecification<TX> inner) => _inner = inner;

			public override Expression<Func<TX, bool>> ToExpression()
			{
				var expr = _inner.ToExpression();
				var param = expr.Parameters[0];
				return Expression.Lambda<Func<TX, bool>>(Expression.Not(expr.Body), param);
			}
		}
	}
}
