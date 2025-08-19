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
		private readonly IMapper _mapper;
		public GetTaxByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<TaxDto> Handle(GetTaxByIdQuery request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id,
				true, cancellationToken,
				x => x.PostingLedgerAccount
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy chính sách thuế");
			return _mapper.Map<TaxDto>(entity);
		}
	}
}
