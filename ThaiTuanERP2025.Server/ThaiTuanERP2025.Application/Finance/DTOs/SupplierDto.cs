using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record SupplierDto(
		Guid Id, string Code, string Name, string? ShortName, bool IsActive,
		string? TaxCode, string? WithholdingTaxType, decimal? WithholdingTaxRate,
		string DefaultCurrency, int PaymentTermDays,
		Guid? PostingProfileId, Guid? SupplierGroupId,
		string? Email, string? Phone, string? AddressLine1, string? AddressLine2,
		string? City, string? StateOrProvince, string? PostalCode, string? Country,
		string? Note,
		DateTime CreatedDate, Guid CreatedByUserId, DateTime? ModifiedDate
	);

	public record CreateSupplierRequest(
		string Code, string Name, string DefaultCurrency,
		string? ShortName, string? TaxCode,
		string? WithholdingTaxType, decimal? WithholdingTaxRate,
		int? PaymentTermDays, Guid? PostingProfileId, Guid? SupplierGroupId,
		string? Email, string? Phone, string? AddressLine1, string? AddressLine2,
		string? City, string? StateOrProvince, string? PostalCode, string? Country,
		string? Note
	);

	public record UpdateSupplierRequest(
		string Name, string DefaultCurrency,
		string? ShortName, string? TaxCode,
		string? WithholdingTaxType, decimal? WithholdingTaxRate,
		int? PaymentTermDays, Guid? PostingProfileId, Guid? SupplierGroupId,
		string? Email, string? Phone, string? AddressLine1, string? AddressLine2,
		string? City, string? StateOrProvince, string? PostalCode, string? Country,
		bool IsActive, string? Note
	);
}
