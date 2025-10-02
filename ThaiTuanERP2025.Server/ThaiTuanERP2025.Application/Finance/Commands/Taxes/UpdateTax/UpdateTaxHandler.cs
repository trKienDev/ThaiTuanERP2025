using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
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
				x => x.Id == request.Id, asNoTracking: false, cancellationToken
			);
			if (entity is null) throw new NotFoundException("không tìm thấy chính sách thuế");

			// Kiểm tra tài khoản hạch toán mới có tồn tại
			var postingLedgerAccount = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.PostingLedgerAccountId, asNoTracking: false, cancellationToken
			);
			if (postingLedgerAccount is null) throw new NotFoundException("Tài khoản hạch toán không tồn tại");

			entity.PolicyName = request.PolicyName;
			entity.Rate = request.Rate;	
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
