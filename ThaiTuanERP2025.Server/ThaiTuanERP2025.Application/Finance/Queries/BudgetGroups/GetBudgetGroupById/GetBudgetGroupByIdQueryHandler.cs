using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetGroups.GetBudgetGroupById
{
	public class GetBudgetGroupByIdQueryHandler : IRequestHandler<GetBudgetGroupByIdQuery, BudgetGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetBudgetGroupByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetGroupDto> Handle(GetBudgetGroupByIdQuery request, CancellationToken cancellationToken)
		{
			var budgetGroup = await _unitOfWork.BudgetGroups.GetByIdAsync(request.Id);
			if (budgetGroup == null) throw new NotFoundException("Nhóm ngân sách không tồn tại");
			return _mapper.Map<BudgetGroupDto>(budgetGroup);
		}
	}
}
