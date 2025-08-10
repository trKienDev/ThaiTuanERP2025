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

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.CreateBankAccount
{
	public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, BankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateBankAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BankAccountDto> Handle(CreateBankAccountCommand command, CancellationToken cancellationToken)
		{
			if (await _unitOfWork.BankAccounts.ExistsDuplicateAsync(command.AccountNumber, command.BankName, command.DepartmentId, command.CustomerName, null, cancellationToken))
				throw new ConflictException("Tài khoản ngân hàng đã tồn tại cho chủ thể này");

			var entity = new BankAccount
			{
				Id = Guid.NewGuid(),
				AccountNumber = command.AccountNumber,
				BankName = command.BankName,
				AccountHolder = command.AccountHolder,
				DepartmentId = command.DepartmentId,
				CustomerName = command.CustomerName,
				IsActive = true,
			};

			await _unitOfWork.BankAccounts.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<BankAccountDto>(entity);

		}
	}
}
