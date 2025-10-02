using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.CheckPolicyNameAvailable
{
	public record CheckPolicyNameAvailableQuery(string policyName, Guid? ExcludeId = null) : IRequest<bool>;
}
