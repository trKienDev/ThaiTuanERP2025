using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxByName
{
	public class GetTaxByNameHandler : IRequestHandler<GetTaxByNameQuery, TaxDto?>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetTaxByNameHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<TaxDto?> Handle(GetTaxByNameQuery request, CancellationToken cancellationToken) {
			return await _unitOfWork.Taxes.GetTaxDtoByNameAsync(request.PolicyName.Trim(), cancellationToken);
		}
	}
}
