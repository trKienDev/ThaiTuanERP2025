using System.Linq.Expressions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Specifications;

namespace ThaiTuanERP2025.Domain.Account.Specifications
{
	public sealed class ActiveRoleLinqSpec : LinqSpecification<Role>
	{
		public override Expression<Func<Role, bool>> ToExpression() => r => r.IsActive;
	}
}
