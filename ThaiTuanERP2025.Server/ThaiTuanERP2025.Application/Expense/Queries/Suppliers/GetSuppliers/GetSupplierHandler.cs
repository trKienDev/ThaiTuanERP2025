using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Suppliers.GetSuppliers
{
	public sealed class GetSupplierHandler : IRequestHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetSupplierHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<SupplierDto>> Handle(GetSuppliersQuery query, CancellationToken cancellationToken) {
			var list = await _unitOfWork.Suppliers.SearchAsync(query.Keyword, cancellationToken);
			return list.Select(_mapper.Map<SupplierDto>).ToList();
		}
	}
}
