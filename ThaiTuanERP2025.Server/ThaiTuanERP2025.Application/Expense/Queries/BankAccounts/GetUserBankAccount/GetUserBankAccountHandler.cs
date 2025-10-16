using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.GetUserBankAccount
{
	public sealed class GetUserBankAccountHandler : IRequestHandler<GetUserBankAccountQuery, BankAccountDto?>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetUserBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BankAccountDto?> Handle(GetUserBankAccountQuery query, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.BankAccounts.GetByUserIdAsync(query.Userid, cancellationToken);
			return entity is null ? null : _mapper.Map<BankAccountDto?>(entity);
		} 
	}
}
