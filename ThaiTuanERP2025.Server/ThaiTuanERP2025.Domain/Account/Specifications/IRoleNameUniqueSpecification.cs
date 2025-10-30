using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Specifications;

namespace ThaiTuanERP2025.Domain.Account.Specifications
{
	public interface IRoleNameUniqueSpecification : ISpecification<Role> 
	{
		Task<bool> IsSatisfiedByAsync(Role role, CancellationToken cancellationToken);
	}
}
