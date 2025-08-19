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

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.UpdateCashOutCode
{
	public class UpdateCashOutCodeHandler : IRequestHandler<UpdateCashOutCodeCommand, CashOutCodeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateCashOutCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashOutCodeDto> Handle(UpdateCashOutCodeCommand request, CancellationToken cancellationToken) {
			if (await _unitOfWork.CashOutCodes.AnyAsync(x =>
				x.Code == request.Code &&
				x.Id != request.Id
			)) throw new ConflictException($"Mã dòng tiền ra '{request.Code}' đã tồn tại");

			var entity = await _unitOfWork.CashOutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id,
				asNoTracking: false
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy mã dòng tiền ra");

			entity.Code = request.Code;
			entity.Name = request.Name;
			entity.CashOutGroupId = request.CashOutGroupId;
			entity.PostingLedegerAccoutnId = request.PostingLedgerAccountId;
			entity.Description = request.Description;

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.CashOutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id,
				true, cancellationToken,
				x => x.CashOutGroup,
				x => x.PostingLedgerAccount
			);
			if (loaded is null) throw new AppException("Có lỗi trong quá trình tạo dòng tiền ra");
			return _mapper.Map<CashOutCodeDto>(loaded);
		}
	}
}
