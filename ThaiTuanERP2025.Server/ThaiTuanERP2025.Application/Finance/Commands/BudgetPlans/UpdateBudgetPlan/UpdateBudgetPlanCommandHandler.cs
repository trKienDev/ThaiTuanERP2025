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

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.UpdateBudgetPlan
{
	public class UpdateBudgetPlanCommandHandler : IRequestHandler<UpdateBudgetPlanCommand, BudgetPlanDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateBudgetPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetPlanDto> Handle(UpdateBudgetPlanCommand command, CancellationToken cancellationToken) {
			var plan = await _unitOfWork.BudgetPlans.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại.");

			plan.Amount = command.Amount;
			plan.Status = command.Status;

			_unitOfWork.BudgetPlans.Update(plan);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<BudgetPlanDto>(plan);
		}
	}
}
