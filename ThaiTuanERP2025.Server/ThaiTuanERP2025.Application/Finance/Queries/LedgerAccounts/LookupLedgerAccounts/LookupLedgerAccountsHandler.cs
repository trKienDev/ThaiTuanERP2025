using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccounts.LookupLedgerAccounts
{
	public sealed class LookupLedgerAccountsHandler : IRequestHandler<LookupLedgerAccountsQuery, List<LedgerAccountLookupDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public LookupLedgerAccountsHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<LedgerAccountLookupDto>> Handle(LookupLedgerAccountsQuery request, CancellationToken cancellationToken) {
			return await _unitOfWork.LedgerAccounts.LookupAsync(request.Keyword, request.Take, cancellationToken);
		}
	}
}
