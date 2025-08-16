using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.Suppliers.ToggleSupplierStatus
{
	public class ToggleSupplierStatusCommandHandler : IRequestHandler<ToggleSupplierStatusCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public ToggleSupplierStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(ToggleSupplierStatusCommand command, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Suppliers.GetByIdAsync(command.Id, cancellationToken);
			if (entity == null)
				throw new KeyNotFoundException($"Nhà cung cấp với ID {command.Id} không tồn tại");
			entity.IsActive = !entity.IsActive;
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<SupplierDto>(entity);
		}
	}
}
