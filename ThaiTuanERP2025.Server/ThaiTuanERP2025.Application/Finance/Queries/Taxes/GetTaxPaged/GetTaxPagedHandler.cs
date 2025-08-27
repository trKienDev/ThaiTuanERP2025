using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetAllTaxes;

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxPaged
{
	public class GetTaxPagedHandler : IRequestHandler<GetTaxPagedQuery, PagedResult<TaxDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetTaxPagedHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PagedResult<TaxDto>> Handle(GetTaxPagedQuery request, CancellationToken cancellationToken) {
			// Uỷ thác projection + filter + sort + paging xuống repo (Infrastructure)
			return await _unitOfWork.Taxes.GetPagedDtosAsync(request.Request, cancellationToken);
		}
	}
}
