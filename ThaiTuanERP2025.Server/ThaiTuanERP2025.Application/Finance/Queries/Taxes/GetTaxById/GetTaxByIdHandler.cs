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

namespace ThaiTuanERP2025.Application.Finance.Queries.Taxes.GetTaxById
{
	public class GetTaxByIdHandler : IRequestHandler<GetTaxByIdQuery, TaxDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetTaxByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<TaxDto> Handle(GetTaxByIdQuery request, CancellationToken cancellationToken) {
			var dto = await _unitOfWork.Taxes.GetTaxDtoByIdAsync(request.Id, cancellationToken);
			if (dto is null) throw new NotFoundException("Không tìm thấy chính sách thuế");
			return dto;
		}
	}
}
