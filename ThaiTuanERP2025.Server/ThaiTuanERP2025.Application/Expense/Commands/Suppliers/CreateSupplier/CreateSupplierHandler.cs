using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.Suppliers.CreateSupplier
{
	public sealed class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateSupplierHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SupplierDto> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			// chống trùng tên (case-insensitive)
			if (await _unitOfWork.Suppliers.ExistsByNameAsync(request.Name))
				throw new ConflictException("Tên nhà cung cấp này đã tồn tại");

			var entity = new Supplier(request.Name, request.TaxCode);
			await _unitOfWork.Suppliers.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<SupplierDto>(entity);
		}
	}
}
