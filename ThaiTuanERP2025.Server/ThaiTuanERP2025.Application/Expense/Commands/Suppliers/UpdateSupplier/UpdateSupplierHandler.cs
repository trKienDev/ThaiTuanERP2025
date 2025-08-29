using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Commands.Suppliers.UpdateSupplier
{
	public sealed class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateSupplierHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;

			var entity = await _unitOfWork.Suppliers.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Không tìm thấy nhà cung cấp");

			entity.Rename(request.Name);
			entity.SetTaxCode(request.TaxCode);
			if (request.IsActive) entity.Activate();
			else entity.Deactive();

			_unitOfWork.Suppliers.Update(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<SupplierDto>(entity);
		}
	}
}
