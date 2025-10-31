using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class InvoiceMappingProfile : Profile
	{
		public InvoiceMappingProfile() {

			CreateMap<Invoice, InvoiceDto>()
				.ForMember(d => d.InvoiceFiles, o => o.MapFrom(s => s.Files))
				.ForMember(d => d.CreatedByUser, o => o.MapFrom(s => s.CreatedByUser));

			CreateMap<InvoiceFile, InvoiceFileDto>()
				.ForMember(d => d.ObjectKey, o => o.MapFrom(s => s.File.ObjectKey));

			CreateMap<InvoiceFile, InvoiceFileDetailDto>();
		}
	}
}
