using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllBudgetCodesQuery
{
	public class GetAllBudgetCodesQueryHandler : IRequestHandler<GetAllBudgetCodesQuery, List<BudgetCodeDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllBudgetCodesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BudgetCodeDto>> Handle(GetAllBudgetCodesQuery request, CancellationToken cancellationToken)
		{
			var budgetCodes = await _unitOfWork.BudgetCodes.GetAllAsync();
			return _mapper.Map<List<BudgetCodeDto>>(budgetCodes);
		}
	}
}
