using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Suppliers.GetSupplierById
{
	public sealed class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetSupplierByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken) {
			var result = await _unitOfWork.Suppliers.GetByIdAsync(query.Id)
				?? throw new DirectoryNotFoundException("Không tim thấy nhà cung cấp");

			return _mapper.Map<SupplierDto>(result);
		}
	}
}
