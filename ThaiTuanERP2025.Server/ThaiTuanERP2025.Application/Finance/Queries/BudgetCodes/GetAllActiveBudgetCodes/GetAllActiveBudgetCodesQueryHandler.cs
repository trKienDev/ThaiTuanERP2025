using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllActiveBudgetCodes
{
	public class GetAllActiveBudgetCodesQueryHandler : IRequestHandler<GetAllActiveBudgetCodesQuery, List<BudgetCodeDto>> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllActiveBudgetCodesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BudgetCodeDto>> Handle(GetAllActiveBudgetCodesQuery query, CancellationToken cancellationToken) {
			var entities = await _unitOfWork.BudgetCodes.FindAsync(x => x.IsActive);
			return _mapper.Map<List<BudgetCodeDto>>(entities);
		}
	}
}
