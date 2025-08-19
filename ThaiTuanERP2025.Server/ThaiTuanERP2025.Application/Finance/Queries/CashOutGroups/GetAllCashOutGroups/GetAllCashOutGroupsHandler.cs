using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.CashOutGroups.GetAllCashOutGroups
{
	public class GetAllCashOutGroupsHandler : IRequestHandler<GetAllCashOutGroupsQuery, List<CashOutGroupDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;	
		public GetAllCashOutGroupsHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<CashOutGroupDto>> Handle(GetAllCashOutGroupsQuery request, CancellationToken cancellationToken) {
			var list = await _unitOfWork.CashOutGroups.FindIncludingAsync(_ => true);
			var ordered = list.OrderBy(x => x.Name).ToList();
			return _mapper.Map<List<CashOutGroupDto>>(ordered);
		}
	}
}
