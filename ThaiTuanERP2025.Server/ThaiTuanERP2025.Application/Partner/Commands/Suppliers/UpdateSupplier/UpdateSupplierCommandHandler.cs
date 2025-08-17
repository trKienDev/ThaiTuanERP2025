using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Partner.Commands.Suppliers.UpdateSupplier
{
	 public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Suppliers.GetByIdAsync(command.Id, cancellationToken);
			if (entity == null)
				throw new NotFoundException($"Nhà cung cấp với ID {command.Id} không tồn tại");

			_mapper.Map(command.request, entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<SupplierDto>(entity);
		}

	}
}
