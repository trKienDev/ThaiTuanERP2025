using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccountTypes.GetLedgerAccountTypeById
{
	public record GetLedgerAccountTypeByIdQuery(Guid Id) : IRequest<LedgerAccountTypeDto>;
}
