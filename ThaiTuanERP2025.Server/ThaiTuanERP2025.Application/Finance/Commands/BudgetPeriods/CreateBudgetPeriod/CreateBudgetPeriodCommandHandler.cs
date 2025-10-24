using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeridos.CreateBudgetPeriod
{
	public class CreateBudgetPeriodCommandHandler : IRequestHandler<CreateBudgetPeriodCommand, BudgetPeriodDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateBudgetPeriodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetPeriodDto> Handle(CreateBudgetPeriodCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var exists = await _unitOfWork.BudgetPeriods.AnyAsync(
				x => x.Year == request.Year && x.Month == request.Month
			);

			if (exists) throw new ConflictException("Kỳ ngân sách đã tồn tại");

			var entity = new BudgetPeriod (
				request.Year,
				request.Month,
				request.BudgetPreparationDate
			);

			await _unitOfWork.BudgetPeriods.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<BudgetPeriodDto>(entity);
		}
	}
}
