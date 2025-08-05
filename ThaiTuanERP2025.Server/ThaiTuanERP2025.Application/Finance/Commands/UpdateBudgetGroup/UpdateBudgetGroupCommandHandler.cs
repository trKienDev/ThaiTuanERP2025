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

namespace ThaiTuanERP2025.Application.Finance.Commands.UpdateBudgetGroup
{
	public class UpdateBudgetGroupCommandHandler : IRequestHandler<UpdateBudgetGroupCommand, BudgetGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;	

		public UpdateBudgetGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetGroupDto> Handle(UpdateBudgetGroupCommand request, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.BudgetGroups.GetByIdAsync(request.Id)
				?? throw new NotFoundException($"Không tìm thấy nhóm ngân sách");
			
			entity.Code = request.Code.Trim().ToUpper();
			entity.Name = request.Name.Trim();

			_unitOfWork.BudgetGroups.Update(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<BudgetGroupDto>(entity);
		}
	}
}
