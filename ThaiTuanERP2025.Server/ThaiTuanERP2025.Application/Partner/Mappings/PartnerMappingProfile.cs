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
			CreateMap<Supplier, SupplierDto>();
			CreateMap<CreateSupplierRequest, Supplier>()
				.ForMember(d => d.PaymentTermDays, o => o.MapFrom(s => s.PaymentTermDays ?? 30))
				.ForMember(d => d.DefaultCurrency, o => o.MapFrom(s => s.DefaultCurrency.ToUpperInvariant()))
				.ForMember(d => d.Code, o => o.MapFrom(s => s.Code.ToUpperInvariant()));
			CreateMap<UpdateSupplierRequest, Supplier>()
				.ForMember(d => d.Code, o => o.Ignore())
				.ForMember(d => d.DefaultCurrency, o => o.MapFrom(s => s.DefaultCurrency.ToUpperInvariant()));

			CreateMap<PartnerBankAccount, PartnerBankAccountDto>();
			CreateMap<UpsertPartnerBankAccountRequest, PartnerBankAccountDto>();
		}
	}
}
