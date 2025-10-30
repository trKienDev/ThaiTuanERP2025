using System.Linq.Expressions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Specifications;

namespace ThaiTuanERP2025.Domain.Account.Specifications
{
	public sealed class RoleHasPermissionLinqSpec : LinqSpecification<Role>
	{
		private readonly Guid _permissionId;
		public RoleHasPermissionLinqSpec(Guid permissionId) => _permissionId = permissionId;

		public override Expression<Func<Role, bool>> ToExpression()  => r => r.RolePermissions.Any(rp => rp.PermissionId == _permissionId);
	}
}
