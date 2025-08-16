using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetPeriods.GetAllBudgetPeriods
{
	public class GetAllBudgetPeriodsQueryHandler : IRequestHandler<GetAllBudgetPeriodsQuery, List<BudgetPeriodDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllBudgetPeriodsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BudgetPeriodDto>> Handle(GetAllBudgetPeriodsQuery request, CancellationToken cancellationToken)
		{
			var entities = await _unitOfWork.BudgetPeriods.GetAllAsync();
			return _mapper.Map<List<BudgetPeriodDto>>(entities);
		}
	}
}
