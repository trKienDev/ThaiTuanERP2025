using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.Suppliers.CreateSupplier
{
	public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
		{
			var request = command.request;
			var code = request.Code.ToUpperInvariant();

			if(await _unitOfWork.Suppliers.ExistsByCodeAsync(code, cancellationToken))
				throw new ConflictException($"Mã nhà cung cấp {code} đã tồn tại");

			var entity = _mapper.Map < Supplier>(request);
			entity.Id = Guid.NewGuid();
			entity.Code = code;

			await _unitOfWork.Suppliers.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<SupplierDto>(entity);
		}
	}
}
