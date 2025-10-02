using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.CreateSupplierBankAccount
{
	public sealed class CreateSupplierBankAccountHandler : IRequestHandler<CreateSupplierBankAccountCommand, BankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateSupplierBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;	
		}

		public async Task<BankAccountDto> Handle(CreateSupplierBankAccountCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var entity = new BankAccount
			{
				Id = Guid.NewGuid(),
				SupplierId = request.SupplierId,
				UserId = null,
				BankName= request.BankName,
				AccountNumber= request.AccountNumber,
				BeneficiaryName= request.BeneficiaryName,
				IsActive = true
			};
			
			await _unitOfWork.BankAccounts.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<BankAccountDto>(entity);
		}
	}
}
