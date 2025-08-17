using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Partner.DTOs;

namespace ThaiTuanERP2025.Application.Partner.Commands.PartnerBankAccounts.UpsertPartnerBankAccountForSupplier
{
	public record UpsertPartnerBankAccountForSupplierCommand(Guid supplierId, UpsertPartnerBankAccountRequest Request) : IRequest<PartnerBankAccountDto>;
}
