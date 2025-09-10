using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.CreateCashoutCode;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Common.Utils;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.CreateCashoutCode
{
	public class CreateCashoutCodeHandler : IRequestHandler<CreateCashoutCodeCommand, CashoutCodeDto> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateCashoutCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashoutCodeDto> Handle(CreateCashoutCodeCommand request, CancellationToken cancellationToken) {
			var name = request.Name?.Trim();
			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Name", "Tên là bắt buộc");

			// Tự sinh code nếu không có
			var code = string.IsNullOrWhiteSpace(request.Code) ? CodeGenerator.FromName(name) : request.Code.Trim();

			if (await _unitOfWork.CashoutCodes.AnyAsync(x =>
				x.Code == request.Code
			)) throw new ConflictException($"Tài khoản đầu ra '{request.Code}' đã tồn tại");

			var cashoutGroup = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.CashoutGroupId
			);
			if (cashoutGroup is null) throw new NotFoundException("Không tìm thấy Nhóm tài khoản đầu ra");

			var postingAccount = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.PostingLedgerAccountId
			);
			if (postingAccount is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");

			var entity = new CashoutCode
			{
				Code = code,
				Name = name,
				CashoutGroupId = request.CashoutGroupId,
				PostingLedgerAccountId = request.PostingLedgerAccountId,
				Description = request.Description,
				IsActive = true
			};
			
			await _unitOfWork.CashoutCodes.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.CashoutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == entity.Id,
				true, cancellationToken,
				x => x.CashoutGroup,
				x => x.PostingLedgerAccount
			);
			if (loaded is null) throw new NotFoundException("Có lỗi xảy ra khi tạo tài khoản đầu ra");
			return _mapper.Map<CashoutCodeDto>(loaded);
		}
	}
}
