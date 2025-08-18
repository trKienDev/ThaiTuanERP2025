using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Partner.Entities;

namespace ThaiTuanERP2025.Application.Partner.Mappings
{
	public class PartnerMappingProfile : Profile
	{
		public PartnerMappingProfile() {
			CreateMap<Supplier, SupplierDto>()
				.ForMember(d => d.PaymentTermDays, o => o.MapFrom(s => s.PaymentTermDays ?? 0));

			CreateMap<CreateSupplierRequest, Supplier>()
				.ForMember(d => d.PaymentTermDays, o => o.MapFrom(s => s.PaymentTermDays ?? 30))
				.ForMember(d => d.DefaultCurrency, o => o.MapFrom(s => s.DefaultCurrency.ToUpperInvariant()))
				.ForMember(d => d.Code, o => o.MapFrom(s => s.Code.ToUpperInvariant()));
			CreateMap<UpdateSupplierRequest, Supplier>()
				.ForMember(d => d.Code, o => o.Ignore())
				.ForMember(d => d.DefaultCurrency, o => o.MapFrom(s => s.DefaultCurrency.ToUpperInvariant()));

			CreateMap<PartnerBankAccount, PartnerBankAccountDto>();

			CreateMap<UpsertPartnerBankAccountRequest, PartnerBankAccount>()
				// Không cho ghi các field khóa/audit/nav
				.ForMember(d => d.Id, o => o.Ignore())
				.ForMember(d => d.SupplierId, o => o.Ignore())
				.ForMember(d => d.Supplier, o => o.Ignore())
				.ForMember(d => d.CreatedDate, o => o.Ignore())
				.ForMember(d => d.DateModified, o => o.Ignore())
				.ForMember(d => d.IsDeleted, o => o.Ignore())
				.ForMember(d => d.DeletedDate, o => o.Ignore())
				.ForMember(d => d.CreatedByUser, o => o.Ignore())
				.ForMember(d => d.ModifiedByUser, o => o.Ignore())
				.ForMember(d => d.DeletedByUser, o => o.Ignore())
				// Chuẩn hóa dữ liệu nếu cần
				.AfterMap((src, dest) =>
				{
					if (!string.IsNullOrWhiteSpace(src.SwiftCode))
						dest.SwiftCode = src.SwiftCode!.ToUpperInvariant().Trim();

					dest.AccountNumber = src.AccountNumber.Trim();
					dest.BankName = src.BankName.Trim();
					if (!string.IsNullOrWhiteSpace(src.AccountHolder))
						dest.AccountHolder = src.AccountHolder!.Trim();
					if (!string.IsNullOrWhiteSpace(src.Branch))
						dest.Branch = src.Branch!.Trim();
					if (!string.IsNullOrWhiteSpace(src.Note))
						dest.Note = src.Note!.Trim();
				});

		}
	}
}
