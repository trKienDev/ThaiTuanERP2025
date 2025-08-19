using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetAllTaxes
{
	public class GetAllTaxesHandler : IRequestHandler<GetAllTaxesQuery, List<TaxDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllTaxesHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;	
		}

		public async Task<List<TaxDto>> Handle(GetAllTaxesQuery request, CancellationToken cancellationToken) {
			var list = await _unitOfWork.Taxes.FindIncludingAsync(
				_ => true,
				x => x.PostingLedgerAccount
			);
			var ordered = list.OrderBy(x => x.PolicyName).ToList();
			return _mapper.Map<List<TaxDto>>(ordered);
		}
	}
}
