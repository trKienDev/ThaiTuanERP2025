using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record TaxDto(
		Guid Id, string PolicyName, decimal Rate, TaxBroadType TaxBoardType, ConsumptionSubType? ConsumptionSubType,
		Guid PostingAccountId, string PostingAccountNumber, string PostingAccountName, 
		string? Description, bool IsActive
	);
}
