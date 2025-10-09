using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class InvoiceMappingProfile : Profile
	{
		public InvoiceMappingProfile() {
			CreateMap<InvoiceLine, InvoiceLineDto>();

			CreateMap<Invoice, InvoiceDto>()
				.ForMember(d => d.FollowerUserIds, o => o.MapFrom(s => s.Follwers.Select(f => f.UserId)))
				.ForMember(d => d.InvoiceLines, o => o.MapFrom(s => s.Lines))
				.ForMember(d => d.SubTotal, o => o.MapFrom(s => s.Lines.Sum(l => l.NetAmount)))
				.ForMember(d => d.TotalVAT, o => o.MapFrom(s => s.Lines.Sum(l => l.VATAmount ?? 0)))
				.ForMember(d => d.TotalWHT, o => o.MapFrom(s => s.Lines.Sum(l => l.WHTAmount ?? 0)))
				.ForMember(d => d.GrandTotal, o => o.MapFrom(s => s.Lines.Sum(l => l.LineTotal)))
				.ForMember(d => d.InvoiceFiles, o => o.MapFrom(s => s.Files));

			CreateMap<InvoiceFile, InvoiceFileDto>()
				.ForMember(d => d.ObjectKey, o => o.MapFrom(s => s.File.ObjectKey));

			CreateMap<InvoiceFile, InvoiceFileDetailDto>();
		}
	}
}
