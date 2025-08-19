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

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccountTypes.GetLedgerAccountTypeById.GetAccountTypeByIdQuery
{
	public class GetLedgerAccountTypeByIdHandler : IRequestHandler<GetLedgerAccountTypeByIdQuery, LedgerAccountTypeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetLedgerAccountTypeByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<LedgerAccountTypeDto> Handle(GetLedgerAccountTypeByIdQuery request, CancellationToken cancellationToken)
		{
			var accountType = await _unitOfWork.LedgerAccountTypes.GetByIdAsync(request.Id);
				?? throw new NotFoundException($"Không tìm thấy tài khoản kế toán với ID: '{request.Id}'");
			return _mapper.Map<LedgerAccountTypeDto>(accountType);
		}
	}
}
