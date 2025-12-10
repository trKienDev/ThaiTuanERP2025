using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Specifications;

namespace ThaiTuanERP2025.Domain.Account.Specifications
{
	public interface IRoleNameUniqueSpecification : ISpecification<Role> 
	{
		Task<bool> IsSatisfiedByAsync(Role role, CancellationToken cancellationToken);
	}
}
