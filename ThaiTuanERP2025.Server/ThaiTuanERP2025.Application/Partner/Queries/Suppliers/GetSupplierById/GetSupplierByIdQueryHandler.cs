using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Partner.Commands.Suppliers.ToggleSupplierStatus;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Partner.Queries.Suppliers.GetSupplierById
{
	public class GetSupplierByIdQueryHandler : IRequestHandler<ToggleSupplierStatusCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetSupplierByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(ToggleSupplierStatusCommand command, CancellationToken cancellationToken)
		{
			var supplier = await _unitOfWork.Suppliers.GetByIdAsync(command.Id);
			if (supplier == null) throw new NotFoundException($"Id nhà cung cấp: {command.Id} không tồn tại.");

			return _mapper.Map<SupplierDto>(supplier);
		}
	}
}
