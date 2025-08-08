using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.CreateBudgetPlan
{
	public class CreateBudgetPlanCommandHandler : IRequestHandler<CreateBudgetPlanCommand, BudgetPlanDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateBudgetPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetPlanDto> Handle(CreateBudgetPlanCommand request, CancellationToken cancellationToken)
		{
			var period = await _unitOfWork.BudgetPeriods.GetByIdAsync(request.BudgetPeriodId)
				?? throw new NotFoundException("Không tìm thấy kỳ ngân sách");
			if (!period.IsActive)
				throw new AppException("Kỳ ngân sách bị vô hiệu hóa");

			var code = await _unitOfWork.BudgetCodes.GetByIdAsync(request.BudgetCodeId)
				?? throw new NotFoundException("Không tìm thấy mã ngân sách");
			if(!code.IsActive)
				throw new AppException("Mã ngân sách bị vô hiệu hóa");

			var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId)
				?? throw new NotFoundException("Không tìm thấy phòng ban");

			var plan = new BudgetPlan
			{
				BudgetPeriodId = request.BudgetPeriodId,
				BudgetCodeId = request.BudgetCodeId,
				DepartmentId = request.DepartmentId,
				Amount = request.Amount,
				Status = "Draft",
			};

			await _unitOfWork.BudgetPlans.AddAsync(plan);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<BudgetPlanDto>(plan);
		}
	}
}
