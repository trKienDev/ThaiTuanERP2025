using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetGroups.GetAllBudgetGroups
{
	public class GetAllBudgetGroupsQueryHandler : IRequestHandler<GetAllBudgetGroupsQuery, List<BudgetGroupDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllBudgetGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<List<BudgetGroupDto>> Handle(GetAllBudgetGroupsQuery request, CancellationToken cancellationToken)
		{
			var budgetGroups = await _unitOfWork.BudgetGroups.GetAllAsync();
			return _mapper.Map<List<BudgetGroupDto>>(budgetGroups);
		}
	}
}