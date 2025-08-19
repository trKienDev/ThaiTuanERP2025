using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Queries.CashOutCodes.GetCashOutCodeById
{
	public class GetCashOutCodeByIdHandler : IRequestHandler<GetCashOutCodeByIdQuery, CashOutCodeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetCashOutCodeByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashOutCodeDto> Handle(GetCashOutCodeByIdQuery request, CancellationToken cancellationToken) { 
			var entity = await _unitOfWork.CashOutCodes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id,
				true, cancellationToken,
				x => x.CashOutGroup, 
				x => x.PostingLedgerAccount
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy mã dòng tiền ra");
			return _mapper.Map<CashOutCodeDto>(entity);
		}
	}
}
