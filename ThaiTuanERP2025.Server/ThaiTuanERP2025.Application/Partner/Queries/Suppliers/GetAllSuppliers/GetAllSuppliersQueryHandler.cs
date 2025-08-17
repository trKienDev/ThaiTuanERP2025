using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Partner.DTOs;

namespace ThaiTuanERP2025.Application.Partner.Queries.Suppliers.GetAllSuppliers
{
	public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, PagedResult<SupplierDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllSuppliersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PagedResult<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
		{
			var (items, total) = await _unitOfWork.Suppliers.SearchAsync(
				keyword: request.Keyword,
				isActive: request.IsActive,
				currency: request.Currency,
				page: request.Page,
				pageSize: request.PageSize,
				cancellationToken: cancellationToken
			);
			var dtos = _mapper.Map<List<SupplierDto>>(items);
			return new PagedResult<SupplierDto>(dtos, total, request.Page, request.PageSize);
		}
	}
}
