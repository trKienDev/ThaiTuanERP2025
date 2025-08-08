using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetPlans.GetAllBudgetPlans
{
	public class GetAllBudgetPlansQueryHandler : IRequestHandler<GetAllBudgetPlansQuery, List<BudgetPlanDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllBudgetPlansQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BudgetPlanDto>> Handle(GetAllBudgetPlansQuery request, CancellationToken cancellationToken)
		{
			var entities = await _unitOfWork.BudgetPlans.GetAllAsync();
			return _mapper.Map<List<BudgetPlanDto>>(entities);
		}

	}
}
