using AutoMapper;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Suppliers
{
	public sealed class SupplierMappingProfile : Profile
	{
		public SupplierMappingProfile() {
			CreateMap<Supplier, SupplierDto>();
		} 
	}
}
