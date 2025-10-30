using System.Linq.Expressions;

namespace ThaiTuanERP2025.Domain.Common.Specifications
{
	/// <summary>
	/// Tiện ích để kết hợp Expression<Func<T,bool>> với AndAlso/OrElse
	/// mà vẫn giữ 1 parameter duy nhất (EF Core có thể dịch tốt).
	/// </summary>
	public static class ExpressionExtensions
	{
		public static Expression<Func<T, bool>> AndAlso<T>(
		    this Expression<Func<T, bool>> left,
		    Expression<Func<T, bool>> right)
		{
			var param = left.Parameters[0];
			var rightBody = new ParameterReplacer(right.Parameters[0], param).Visit(right.Body);
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, rightBody!), param);
		}

		public static Expression<Func<T, bool>> OrElse<T>(
		    this Expression<Func<T, bool>> left,
		    Expression<Func<T, bool>> right)
		{
			var param = left.Parameters[0];
			var rightBody = new ParameterReplacer(right.Parameters[0], param).Visit(right.Body);
			return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, rightBody!), param);
		}

		private sealed class ParameterReplacer : ExpressionVisitor
		{
			private readonly ParameterExpression _from;
			private readonly ParameterExpression _to;
			public ParameterReplacer(ParameterExpression from, ParameterExpression to)
			{
				_from = from; _to = to;
			}
			protected override Expression VisitParameter(ParameterExpression node)
			    => node == _from ? _to : base.VisitParameter(node);
		}
	}
}
