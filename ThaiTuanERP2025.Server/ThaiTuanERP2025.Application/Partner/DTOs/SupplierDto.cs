using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Partner.DTOs
{
	public class SupplierDto
	{
		public Guid Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? ShortName { get; set; }
		public bool IsActive { get; set; }

		public string? TaxCode { get; set; }
		public string? WithholdingTaxType { get; set; }
		public decimal? WithholdingTaxRate { get; set; }

		public string DefaultCurrency { get; set; } = null!;
		public int PaymentTermDays { get; set; } // nếu entity là int?, bạn map ?? 0 ở profile

		public Guid? PostingProfileId { get; set; }
		public Guid? SupplierGroupId { get; set; }

		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? AddressLine1 { get; set; }
		public string? AddressLine2 { get; set; }
		public string? City { get; set; }
		public string? StateOrProvince { get; set; }
		public string? PostalCode { get; set; }
		public string? Country { get; set; }
		public string? Note { get; set; }

		public DateTime CreatedDate { get; set; }
		public Guid CreatedByUserId { get; set; }
		public DateTime? DateModified { get; set; }
	}

	public record CreateSupplierRequest(
		string? Code, string Name, string DefaultCurrency,
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
