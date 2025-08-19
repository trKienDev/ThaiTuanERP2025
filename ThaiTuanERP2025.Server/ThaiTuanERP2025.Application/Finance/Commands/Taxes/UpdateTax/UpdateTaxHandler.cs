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

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.UpdateTax
{
	public class UpdateTaxHandler : IRequestHandler<UpdateTaxCommand, TaxDto> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateTaxHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<TaxDto> Handle(UpdateTaxCommand request, CancellationToken cancellationToken) {
			if (await _unitOfWork.Taxes.AnyAsync(x =>
				x.PolicyName == request.PolicyName &&
				x.Id != request.Id
			)) throw new ConflictException($"Chính sách thuế '{request.PolicyName}' đã tồn tại");

			var entity = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(
				x => x.PolicyName == request.PolicyName && x.Id == request.Id, asNoTracking: false, cancellationToken
			);
			if (entity is null) throw new NotFoundException("không tìm thấy chính sách thuế");

			entity.PolicyName = request.PolicyName;
			entity.Rate = request.Rate;	
			entity.TaxBroadType = request.TaxBroadType;
			entity.ConsumptionSubType = request.ConsumptionSubType;
			entity.PostingLedgerAccountId = request.PostingLedgerAccountId;	
			entity.Description = request.Description;

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id, true, cancellationToken,
				x => x.PostingLedgerAccount
			);
			if (loaded is null) throw new NotFoundException();
			return _mapper.Map<TaxDto>(loaded);
		}
	}
}
