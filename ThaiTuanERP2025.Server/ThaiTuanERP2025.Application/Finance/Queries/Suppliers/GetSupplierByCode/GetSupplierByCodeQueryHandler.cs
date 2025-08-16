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

namespace ThaiTuanERP2025.Application.Finance.Queries.Suppliers.GetSupplierByCode
{
	public class GetSupplierByCodeQueryHandler : IRequestHandler<GetSupplierByCodeQuery, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetSupplierByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(GetSupplierByCodeQuery request, CancellationToken cancellationToken)
		{
			var supplier = await _unitOfWork.Suppliers.FindByCodeAsync(request.Code.ToUpperInvariant(), cancellationToken);
			if (supplier == null) throw new NotFoundException($"Mã nhà cung cấp: {request.Code} không tồn tại.");
			return _mapper.Map<SupplierDto>(supplier);
		}
	}
}
