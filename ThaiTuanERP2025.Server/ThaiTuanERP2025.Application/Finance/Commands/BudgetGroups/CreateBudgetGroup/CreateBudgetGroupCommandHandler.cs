using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.CreateBudgetGroup
{
	public class CreateBudgetGroupCommandHandler : IRequestHandler<CreateBudgetGroupCommand, BudgetGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateBudgetGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetGroupDto> Handle(CreateBudgetGroupCommand request, CancellationToken cancellationToken)
		{
			var entity = new BudgetGroup
			{
				Id = Guid.NewGuid(),
				Code = request.Code,
				Name = request.Name
			};

			await _unitOfWork.BudgetGroups.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<BudgetGroupDto>(entity);
		}
	}
}
