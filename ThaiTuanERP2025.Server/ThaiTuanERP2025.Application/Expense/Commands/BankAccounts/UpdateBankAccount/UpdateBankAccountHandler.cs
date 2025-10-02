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

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.UpdateBankAccount
{
	public sealed class UpdateBankAccountHandler : IRequestHandler<UpdateBankAccountCommand, BankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BankAccountDto> Handle(UpdateBankAccountCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var entity = await _unitOfWork.BankAccounts.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Không tìm thấy tài khoản ngân hàng được chỉ định");

			entity.BankName = request.BankName.Trim();
			entity.AccountNumber = request.AccountNumber.Trim();
			entity.BeneficiaryName = request.BeneficiaryName.Trim();
			entity.IsActive = request.IsActive;

			_unitOfWork.BankAccounts.Update(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<BankAccountDto>(entity);
		}
	}
}
