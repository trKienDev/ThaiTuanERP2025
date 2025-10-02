using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Queries.CashoutCodes.GetCashoutCodeById
{
	public class GetCashoutCodeByIdHandler : IRequestHandler<GetCashoutCodeByIdQuery, CashoutCodeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetCashoutCodeByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashoutCodeDto> Handle(GetCashoutCodeByIdQuery request, CancellationToken cancellationToken) { 
			var entity = await _unitOfWork.CashoutCodes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id,
				true, cancellationToken,
				x => x.CashoutGroup, 
				x => x.PostingLedgerAccount
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy mã dòng tiền ra");
			return _mapper.Map<CashoutCodeDto>(entity);
		}
	}
}
