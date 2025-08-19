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
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.CreateCashOutCode
{
	public class CreateCashOutCodeHandler : IRequestHandler<CreateCashOutCodeCommand, CashOutCodeDto> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateCashOutCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashOutCodeDto> Handle(CreateCashOutCodeCommand request, CancellationToken cancellationToken) {
			if (await _unitOfWork.CashOutCodes.AnyAsync(x =>
				x.Code == request.Code
			)) throw new ConflictException($"Tài khoản đầu ra '{request.Code}' đã tồn tại");

			var cashOutGroup = await _unitOfWork.CashOutGroups.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.CashOutGroupId
			);
			if (cashOutGroup is null) throw new NotFoundException("Không tìm thấy Nhóm tài khoản đầu ra");

			var postingAccount = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.PostingLedgerAccountId
			);
			if (postingAccount is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");

			var entity = new CashOutCode
			{
				Code = request.Code,
				Name = request.Name,
				CashOutGroupId = request.CashOutGroupId,
				PostingLedegerAccoutnId = request.PostingLedgerAccountId,
				Description = request.Description,
				IsActive = true
			};
			
			await _unitOfWork.CashOutCodes.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.CashOutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == entity.Id,
				true, cancellationToken,
				x => x.CashOutGroup,
				x => x.PostingLedgerAccount
			);
			if (loaded is null) throw new NotFoundException("Có lỗi xảy ra khi tạo tài khoản đầu ra");
			return _mapper.Map<CashOutCodeDto>(loaded);
		}
	}
}
