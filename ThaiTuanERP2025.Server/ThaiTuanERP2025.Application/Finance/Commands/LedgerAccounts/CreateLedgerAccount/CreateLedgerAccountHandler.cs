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
			string parentPath = "/";
			int ParentLevel = -1;
			if(command.ParentLedgerAccountId.HasValue)
			{
				var parentAccount = await _unitOfWork.LedgerAccounts.GetByIdAsync(command.ParentLedgerAccountId.Value)
					?? throw new NotFoundException("Parent account not found");
				parentPath = parentAccount.Path;
				ParentLevel = parentAccount.Level;
			}

			var entity = new LedgerAccount
			{
				Number = command.Number,
				Name = command.Name,
				LedgerAccountTypeId = command.LedgerAccountTypeId,
				ParrentLedgerAccountId = command.ParentLedgerAccountId,
				Path = $"{parentPath}{command.Number}",
				Level = ParentLevel + 1,
				Description = command.Description,
				IsActive = true
			};

			await _unitOfWork.LedgerAccounts.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// include AccountType for mapping name
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
