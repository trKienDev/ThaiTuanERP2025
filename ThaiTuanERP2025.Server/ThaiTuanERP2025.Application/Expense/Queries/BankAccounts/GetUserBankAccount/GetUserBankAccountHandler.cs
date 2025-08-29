using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

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
