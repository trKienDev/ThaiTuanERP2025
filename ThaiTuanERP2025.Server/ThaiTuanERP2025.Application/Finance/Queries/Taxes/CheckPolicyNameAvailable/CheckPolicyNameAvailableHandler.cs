using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.CheckPolicyNameAvailable
{
	public class CheckPolicyNameAvailableHandler : IRequestHandler<CheckPolicyNameAvailableQuery, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CheckPolicyNameAvailableHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(CheckPolicyNameAvailableQuery request, CancellationToken cancellationToken) {
			var name = request.policyName?.Trim() ?? string.Empty;
			if (string.IsNullOrEmpty(name)) return false;

			var exists = await _unitOfWork.Taxes.PolicyNameExistsAsync(name, request.ExcludeId, cancellationToken);
			return !exists;
		}
	}
}
