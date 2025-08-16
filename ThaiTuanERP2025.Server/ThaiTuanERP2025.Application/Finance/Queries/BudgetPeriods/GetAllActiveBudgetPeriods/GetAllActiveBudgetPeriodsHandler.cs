using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetPeriods.GetAllActiveBudgetPeriods
{
	public class GetAllActiveBudgetPeriodsHandler : IRequestHandler<GetAllActiveBudgetPeriodsQuery, List<BudgetPeriodDto>> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllActiveBudgetPeriodsHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BudgetPeriodDto>> Handle(GetAllActiveBudgetPeriodsQuery query, CancellationToken cancellationToken) {
			var entities = await _unitOfWork.BudgetPeriods.FindAsync(x => x.IsActive);
			return _mapper.Map<List<BudgetPeriodDto>>(entities);
		}
	}
}
