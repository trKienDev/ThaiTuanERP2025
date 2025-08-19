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

			entity.Number = command.Number;
			entity.Name = command.Name;
			entity.LedgerAccountTypeId = command.LedgerAccountTypeId;
			entity.Description = command.Description;

			await _unitOfWork.SaveChangesAsync( cancellationToken );

			var reloaded = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				x => x.Id == command.Id, 
				true, cancellationToken,
				x => x.LedgerAccountType
			);
			if (reloaded is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");
			return _mapper.Map<LedgerAccountDto>(reloaded );
		}
	}
}
