using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;

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
			var entities = await _unitOfWork.BudgetPlans.GetAllIncludingAsync(
				cancellationToken,
				x => x.Department,
				x => x.BudgetCode,
				x => x.BudgetPeriod
			);
			return _mapper.Map<List<BudgetPlanDto>>(entities);
		}
	}
}
