using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Services;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.CreateLedgerAccount
{
	public class CreateLedgerAccountHandler : IRequestHandler<CreateLedgerAccountCommand, LedgerAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateLedgerAccountHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<LedgerAccountDto> Handle(CreateLedgerAccountCommand command, CancellationToken cancellationToken)
		{
			// 1) Chuẩn hóa input
			var number = command.Number?.Trim() ?? throw new ArgumentNullException(nameof(command.Number));

			// 2) Tìm parent (nếu có) theo ParrentLedgerAccountId (đánh vần như entity)
			LedgerAccount? parentAccount = null;
			if(command.ParentLedgerAccountId.HasValue)
			{
				parentAccount = await _unitOfWork.LedgerAccounts.GetByIdAsync(command.ParentLedgerAccountId.Value)
					?? throw new NotFoundException("Parent ledger account not found");
			}

			// 3) Khởi tạo entity
			var entity = new LedgerAccount
			{
				Number = command.Number,
				Name = command.Name,
				LedgerAccountBalanceType = command.LedgerAccountBalanceType,
				LedgerAccountTypeId = command.LedgerAccountTypeId,
				ParentLedgerAccountId = command.ParentLedgerAccountId,
				Description = command.Description,
				IsActive = true
			};

			// 4) Đặt Path/Level bằng helper
			LedgerAccountPathHelper.SetPathAndLevel(entity, parentAccount);

			// 5) Lưu
			await _unitOfWork.LedgerAccounts.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// 6) Reload + map DTO include type name
			var reloaded = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				a => a.Id == entity.Id,
				asNoTracking: true,
				cancellationToken: cancellationToken,
				a => a.LedgerAccountType
			);
			if(reloaded is null) throw new NotFoundException("Có lỗi xảy ra khi tạo tài khoản kế toán");

			return _mapper.Map<LedgerAccountDto>(reloaded);
		}

	}
}
