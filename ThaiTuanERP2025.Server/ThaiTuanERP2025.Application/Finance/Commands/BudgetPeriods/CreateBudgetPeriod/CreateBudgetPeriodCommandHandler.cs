using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
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

		public async Task<BudgetPeriodDto> Handle(CreateBudgetPeriodCommand request, CancellationToken cancellationToken)
		{
			var exists = await _unitOfWork.BudgetPeriods.AnyAsync(
				x => x.Year == request.Year && x.Month == request.Month
			);

			if (exists) throw new ConflictException("Kỳ ngân sách đã tồn tại");

			var entity = new BudgetPeriod
			{
				Id = Guid.NewGuid(),
				Year = request.Year,
				Month = request.Month,
				IsActive = true,
				CreatedDate = DateTime.UtcNow
			};

			await _unitOfWork.BudgetPeriods.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<BudgetPeriodDto>(entity);
		}
	}
}
