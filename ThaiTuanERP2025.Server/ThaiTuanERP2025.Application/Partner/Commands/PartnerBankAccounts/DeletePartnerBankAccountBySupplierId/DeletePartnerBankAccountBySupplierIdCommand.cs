using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Partner.Commands.PartnerBankAccounts.DeletePartnerBankAccountBySupplierId
{
	public record DeletePartnerBankAccountBySupplierIdCommand(Guid supplierId) : IRequest<bool>;
}
