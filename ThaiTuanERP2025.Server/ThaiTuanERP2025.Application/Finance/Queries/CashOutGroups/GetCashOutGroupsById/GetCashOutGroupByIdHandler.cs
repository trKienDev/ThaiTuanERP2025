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

namespace ThaiTuanERP2025.Application.Finance.Queries.CashoutGroups.GetCashoutGroupById
{
	public class GetCashoutGroupByIdHandler : IRequestHandler<GetCashoutGroupByIdQuery, CashoutGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetCashoutGroupByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashoutGroupDto> Handle(GetCashoutGroupByIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(x =>x.Id == request.Id);
			if (entity is null) throw new NotFoundException("Không tìm thấy nhóm tài khoản đầu ra");
			return _mapper.Map<CashoutGroupDto>(entity);
		}
	}
}
