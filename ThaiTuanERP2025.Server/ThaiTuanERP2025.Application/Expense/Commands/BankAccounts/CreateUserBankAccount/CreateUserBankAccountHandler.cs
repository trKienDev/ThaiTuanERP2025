using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.CreateUserBankAccount
{
	public sealed class CreateUserBankAccountHandler : IRequestHandler<CreateUserBankAccountCommand, BankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateUserBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BankAccountDto> Handle(CreateUserBankAccountCommand commnad, CancellationToken cancellationToken) {
			var request = commnad.Request;
			if (await _unitOfWork.BankAccounts.ExistsForUserAsync(request.UserId, cancellationToken))
				throw new ConflictException("User này đã có tài khoản ngân hàng");

			var entity = new BankAccount
			{
				Id = Guid.NewGuid(),
				UserId = request.UserId,
				SupplierId = null,
				BankName = request.BankName.Trim(),
				AccountNumber = request.AccountNumber.Trim(),
				BeneficiaryName = request.BeneficiaryName.Trim(),
				IsActive = true
			};

			await _unitOfWork.BankAccounts.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<BankAccountDto>(entity);
		}
	}
}
