using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.CashoutGroups.GetAllCashoutGroups
{
	public class GetAllCashoutGroupsHandler : IRequestHandler<GetAllCashoutGroupsQuery, List<CashoutGroupDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;	
		public GetAllCashoutGroupsHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<CashoutGroupDto>> Handle(GetAllCashoutGroupsQuery request, CancellationToken cancellationToken) {
			var list = await _unitOfWork.CashoutGroups.FindIncludingAsync(_ => true);
			var ordered = list.OrderBy(x => x.Name).ToList();
			return _mapper.Map<List<CashoutGroupDto>>(ordered);
		}
	}
}
