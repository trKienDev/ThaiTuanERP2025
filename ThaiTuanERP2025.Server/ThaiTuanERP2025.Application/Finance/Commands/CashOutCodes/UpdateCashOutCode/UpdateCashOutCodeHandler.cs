using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.UpdateCashoutCode
{
	public class UpdateCashoutCodeHandler : IRequestHandler<UpdateCashoutCodeCommand, CashoutCodeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateCashoutCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashoutCodeDto> Handle(UpdateCashoutCodeCommand request, CancellationToken cancellationToken) {
			if (await _unitOfWork.CashoutCodes.AnyAsync(x =>
				x.Code == request.Code &&
				x.Id != request.Id
			)) throw new ConflictException($"Mã dòng tiền ra '{request.Code}' đã tồn tại");

			var entity = await _unitOfWork.CashoutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id,
				asNoTracking: false
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy mã dòng tiền ra");

			entity.Code = request.Code;
			entity.Name = request.Name;
			entity.CashoutGroupId = request.CashOutGroupId;
			entity.PostingLedgerAccountId = request.PostingLedgerAccountId;
			entity.Description = request.Description;

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.CashoutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id,
				true, cancellationToken,
				x => x.CashoutGroup,
				x => x.PostingLedgerAccount
			);
			if (loaded is null) throw new AppException("Có lỗi trong quá trình tạo dòng tiền ra");
			return _mapper.Map<CashoutCodeDto>(loaded);
		}
	}
}
