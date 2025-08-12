using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BankAccounts.GetAllBankAccounts
{
	public record GetAllBankAccountsQuery (bool? OnlyActive = null, int Page = 1, int PageSize = 20) : IRequest<PagedResult<BankAccountDto>>;
}
