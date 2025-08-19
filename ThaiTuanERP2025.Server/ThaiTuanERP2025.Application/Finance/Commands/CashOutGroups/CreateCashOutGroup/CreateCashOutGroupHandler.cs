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

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.CreateCashOutGroup
{
	public class CreateCashOutGroupHandler : IRequestHandler<CreateCashOutGroupCommand, CashOutGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateCashOutGroupHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashOutGroupDto> Handle(CreateCashOutGroupCommand request, CancellationToken cancellationToken) {
			if (await _unitOfWork.CashOutGroups.AnyAsync(x =>
				x.Code == request.Code
			)) throw new ConflictException($"Nhóm dòng tiền ra '{request.Code}' đã tồn tại");

			var entity = new CashOutGroup
			{
				Code = request.Code,
				Name = request.Name,
				Description = request.Description,
				IsActive = true
			};
			await _unitOfWork.CashOutGroups.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			var loaded = await _unitOfWork.CashOutGroups.SingleOrDefaultIncludingAsync(x => 
				x.Id == entity.Id
			);
			if (loaded is null) throw new NotFoundException();
			return _mapper.Map<CashOutGroupDto>( loaded );

		}
	}
}
