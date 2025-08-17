using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Partner.DTOs
{
	public record PartnerBankAccountDto(
		Guid Id, 
		Guid SupplierId,
		string AccountNumber, 
		string BankName, 
		string? AccountHolder,
		string? SwiftCode, 
		string? Branch, 
		string? Note, 
		bool IsActive,
		DateTime CreatedDate, 
		Guid CreatedByUserId, 
		DateTime? DateModified
	);

	public record UpsertPartnerBankAccountRequest(
		string AccountNumber, 
		string BankName, 
		string? AccountHolder,
		string? SwiftCode, 
		string? Branch, 
		string? Note, 
		bool IsActive
	);
}
