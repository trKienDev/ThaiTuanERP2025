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

namespace ThaiTuanERP2025.Application.Finance.Queries.CashOutGroups.GetCashOutGroupById
{
	public class GetCashOutGroupByIdHandler : IRequestHandler<GetCashOutGroupByIdQuery, CashOutGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetCashOutGroupByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashOutGroupDto> Handle(GetCashOutGroupByIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.CashOutGroups.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy nhóm tài khoản đầu ra");
			return _mapper.Map<CashOutGroupDto>(entity);
		}
	}
}
