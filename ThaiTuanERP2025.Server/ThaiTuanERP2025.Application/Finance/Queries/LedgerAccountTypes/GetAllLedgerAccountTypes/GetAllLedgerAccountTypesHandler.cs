using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccountTypes.GetAllLedgerAccountTypes
{
	public class GetAllLedgerAccountTypesHandler : IRequestHandler<GetAllLedgerAccountTypesQuery, List<LedgerAccountTypeDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllLedgerAccountTypesHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<LedgerAccountTypeDto>> Handle(GetAllLedgerAccountTypesQuery request, CancellationToken cancellationToken)
		{
			var ledgerAccountTypes = await _unitOfWork.LedgerAccountTypes.ListAsync(
				query => query.OrderBy(x => x.Name),
				asNoTracking: true,
				cancellationToken: cancellationToken
			);

			return _mapper.Map<List<LedgerAccountTypeDto>>(ledgerAccountTypes);
		}
	}
}
