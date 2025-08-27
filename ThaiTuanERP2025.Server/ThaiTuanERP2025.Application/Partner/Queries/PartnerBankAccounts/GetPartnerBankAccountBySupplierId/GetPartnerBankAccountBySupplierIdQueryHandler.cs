using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Partner.Queries.PartnerBankAccounts.GetPartnerBankAccountBySupplierId
{
	public class GetPartnerBankAccountBySupplierIdQueryHandler : IRequestHandler<GetPartnerBankAccountBySupplierIdQuery, PartnerBankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetPartnerBankAccountBySupplierIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PartnerBankAccountDto> Handle(GetPartnerBankAccountBySupplierIdQuery request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.PartnerBankAccounts.GetBySupplierIdAsync(request.suppliderId, cancellationToken);
			if (entity is null) throw new NotFoundException($"Nhà cung cấp '{request.suppliderId}' chưa có tài khoản ngân hàng");

			return _mapper.Map<PartnerBankAccountDto>(entity);
		}
	}
}
