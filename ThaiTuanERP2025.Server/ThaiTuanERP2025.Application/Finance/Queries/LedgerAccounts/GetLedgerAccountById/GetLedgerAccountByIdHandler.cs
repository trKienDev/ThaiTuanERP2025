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

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccounts.GetLedgerAccountById
{
	public class GetLedgerAccountByIdHandler : IRequestHandler<GetLedgerAccountByIdQuery, LedgerAccountDto> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetLedgerAccountByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<LedgerAccountDto> Handle(GetLedgerAccountByIdQuery request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				x => x.Id == request.Id,
				true,
				cancellationToken,
				x => x.LedgerAccountType
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");
			return _mapper.Map<LedgerAccountDto>(entity);
		}
	}
}
