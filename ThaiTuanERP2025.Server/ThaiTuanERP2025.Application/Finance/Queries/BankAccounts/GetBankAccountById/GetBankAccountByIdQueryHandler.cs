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

namespace ThaiTuanERP2025.Application.Finance.Queries.BankAccounts.GetBankAccountById
{ 
	public class GetBankAccountByIdQueryHandler : IRequestHandler<GetBankAccountByIdQuery, BankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetBankAccountByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}	

		public async Task<BankAccountDto> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
		{
			var dto = await _unitOfWork.BankAccountRead.GetByIdAsync(request.Id);
			if (dto == null)
				throw new NotFoundException($"Tài khoản ngân hàng với Id: {request.Id} không tồn tại.");
			return dto;
		}
	}
}
