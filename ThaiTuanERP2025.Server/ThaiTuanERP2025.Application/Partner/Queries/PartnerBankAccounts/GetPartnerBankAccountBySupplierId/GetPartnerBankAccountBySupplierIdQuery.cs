using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Partner.DTOs;

namespace ThaiTuanERP2025.Application.Partner.Queries.PartnerBankAccounts.GetPartnerBankAccountBySupplierId
{
	public record GetPartnerBankAccountBySupplierIdQuery(Guid suppliderId) : IRequest<PartnerBankAccountDto>;
}
