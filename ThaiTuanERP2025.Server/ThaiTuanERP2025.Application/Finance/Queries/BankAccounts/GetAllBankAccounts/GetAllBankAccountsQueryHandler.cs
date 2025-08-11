using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Common.Models;

namespace ThaiTuanERP2025.Application.Finance.Queries.BankAccounts.GetAllBankAccounts
{
	public class GetAllBankAccountsQueryHandler : IRequestHandler<GetAllBankAccountsQuery, PagedResult<BankAccountDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetAllBankAccountsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PagedResult<BankAccountDto>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
		{
			var page = request.Page <= 0 ? 1 : request.Page;
			var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;
			return await _unitOfWork.BankAccountRead.SearchPagedAsync(request.OnlyActive, request.DepartmentId, page, pageSize, cancellationToken);
		}

	}
}
