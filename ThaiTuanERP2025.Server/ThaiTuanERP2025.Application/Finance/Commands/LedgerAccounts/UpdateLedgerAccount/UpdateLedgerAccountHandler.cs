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
using ThaiTuanERP2025.Domain.Finance.Services;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.UpdateLedgerAccount
{
	public class UpdateLedgerAccountHandler : IRequestHandler<UpdateLedgerAccountCommand, LedgerAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateLedgerAccountHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<LedgerAccountDto> Handle(UpdateLedgerAccountCommand command, CancellationToken cancellationToken) {
			if (await _unitOfWork.LedgerAccounts.AnyAsync(x =>
				x.Number == command.Number &&
				x.Id != command.Id
			)) throw new ConflictException($"Số tài khoản kế toán '{command.Number}' đã tồn tại");

			var entity = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				x => x.Id == command.Id,
				asNoTracking: false,
				cancellationToken
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");

			// 2) Cập nhật field cơ bản
			entity.Number = command.Number;
			entity.Name = command.Name;
			entity.LedgerAccountTypeId = command.LedgerAccountTypeId;
			entity.Description = command.Description;
			entity.LedgerAccountBalanceType = command.LedgerAccountBalanceType;

			// 3) Nếu có cho đổi cha, cập nhật parent id (đúng chính tả với entity)
			if (command.ParentLedgerAccountId != entity.ParentLedgerAccountId)
			{
				entity.ParentLedgerAccountId = command.ParentLedgerAccountId;
			}

			// 4) Tải parent (nếu có)
			LedgerAccount? parentAccount = null;
			if (entity.ParentLedgerAccountId.HasValue) {
				parentAccount = await _unitOfWork.LedgerAccounts.GetByIdAsync(entity.ParentLedgerAccountId.Value)
					?? throw new NotFoundException("Không tìm thấy tài khoản kế toán cha");
			}

			// 5) Đặt lại Path/Level cho chính node
			LedgerAccountPathHelper.SetPathAndLevel(entity, parentAccount);

			// 6) Cập nhật lại Path/Level cho toàn bộ descendants
			await RebuildDescendantsPathAsync(entity, cancellationToken);

			// 7) Lưu
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// 8) Reload + map
			var reloaded = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				x => x.Id == command.Id,
				true, cancellationToken,
				x => x.LedgerAccountType
			);
			if (reloaded is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");
			return _mapper.Map<LedgerAccountDto>(reloaded);
		}

		/// <summary>
		/// Cập nhật Path/Level cho toàn bộ subtree dựa vào Path/Level của node hiện tại.
		/// </summary>
		private async Task RebuildDescendantsPathAsync(LedgerAccount root, CancellationToken cancellationToken)
		{
			// Lấy toàn bộ children trực tiếp của root
			var children = await _unitOfWork.LedgerAccounts.FindAsync(x => x.ParentLedgerAccountId == root.Id);
			foreach (var child in children)
			{
				child.Path = $"{root.Path.TrimEnd('/')}/{child.Number.Trim()}";
				child.Level = root.Level + 1;

				// đệ quy xuống cấp dưới
				await RebuildDescendantsPathAsync(child, cancellationToken);
			}
		}
	}
}

