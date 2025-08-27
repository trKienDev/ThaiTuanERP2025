using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Partner.DTOs;

namespace ThaiTuanERP2025.Application.Partner.Queries.Suppliers.GetAllSuppliers
{
	public record GetAllSuppliersQuery(string? Keyword, bool? IsActive, string? Currency, int Page = 1, int PageSize = 20) : IRequest<PagedResult<SupplierDto>>;
}
