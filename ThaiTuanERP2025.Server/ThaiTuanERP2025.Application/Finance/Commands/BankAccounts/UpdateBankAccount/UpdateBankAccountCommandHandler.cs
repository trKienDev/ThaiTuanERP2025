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

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.UpdateBankAccount
{
	public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand, BankAccountDto> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BankAccountDto> Handle(UpdateBankAccountCommand command, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.BankAccounts.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Tài khoản ngân hàng không tồn tại");

			if (await _unitOfWork.BankAccounts.ExistsDuplicateAsync(command.AccountNumber, command.BankName, command.DepartmentId, command.CustomerName, command.Id, cancellationToken))
				throw new ConflictException("Khách hàng đã có tài khoản ngân hàng này");

			entity.AccountNumber = command.AccountNumber;
			entity.BankName = command.BankName;
			entity.AccountHolder = command.AccountHolder;
			entity.DepartmentId = command.DepartmentId;
			entity.CustomerName = command.CustomerName;

			_unitOfWork.BankAccounts.Update(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<BankAccountDto>(entity);
		}
	}
}
