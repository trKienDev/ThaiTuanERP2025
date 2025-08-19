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

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.UpdateCashOutGroup
{
	public class UpdateCashOutGroupHandler : IRequestHandler<UpdateCashOutGroupCommand, CashOutGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateCashOutGroupHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashOutGroupDto> Handle(UpdateCashOutGroupCommand request, CancellationToken cancellationToken) {
			if (await _unitOfWork.CashOutGroups.AnyAsync(x =>
				x.Code == request.Code &&
				x.Id != request.Id
			)) throw new ConflictException("Nhóm tài khoản đầu ra đã tồn tại");

			var entity = await _unitOfWork.CashOutGroups.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id,
				asNoTracking: false
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy nhóm tài khoản đầu ra");

			var loaded = await _unitOfWork.CashOutGroups.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id
			);
			if (loaded is null) throw new NotFoundException();
			return _mapper.Map<CashOutGroupDto>(loaded);
		}
	}
}
