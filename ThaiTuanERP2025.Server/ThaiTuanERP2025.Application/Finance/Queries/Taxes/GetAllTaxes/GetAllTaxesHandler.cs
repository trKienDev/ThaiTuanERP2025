using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetAllTaxes
{
	public class GetAllTaxesHandler : IRequestHandler<GetAllTaxesQuery, List<TaxDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetAllTaxesHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<TaxDto>> Handle(GetAllTaxesQuery request, CancellationToken cancellationToken) {
			return await _unitOfWork.Taxes.ListTaxDtosAsync(request.IsActive, request.Search, cancellationToken);
		}
	}
}
