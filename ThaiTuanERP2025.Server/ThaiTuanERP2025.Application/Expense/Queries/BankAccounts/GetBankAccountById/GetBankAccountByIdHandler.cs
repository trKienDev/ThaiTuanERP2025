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

namespace ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.GetBankAccountById
{
	public class GetBankAccountByIdHandler : IRequestHandler<GetBankAccountByIdQuery, BankAccountDto> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetBankAccountByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) { 
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BankAccountDto> Handle(GetBankAccountByIdQuery query, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.BankAccounts.GetByIdAsync(query.Id)
				?? throw new NotFoundException("Không tìm thấy tài khoản ngân hàng của user này");
			return _mapper.Map<BankAccountDto>(entity);
		}
	}
}
