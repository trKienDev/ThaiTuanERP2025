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
using ThaiTuanERP2025.Domain.Partner.Entities;

namespace ThaiTuanERP2025.Application.Partner.Commands.PartnerBankAccounts.UpsertPartnerBankAccountForSupplier
{
	public class UpsertPartnerBankAccountForSupplierHandler : IRequestHandler<UpsertPartnerBankAccountForSupplierCommand, PartnerBankAccountDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpsertPartnerBankAccountForSupplierHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PartnerBankAccountDto> Handle(UpsertPartnerBankAccountForSupplierCommand command, CancellationToken cancellationToken) {
			var supplier = await _unitOfWork.Suppliers.GetByIdAsync(command.supplierId, cancellationToken);
			if (supplier is null) throw new NotFoundException($"Nhà cung cấp '{command.supplierId}' không tồn tại");

			var existing = await _unitOfWork.PartnerBankAccounts.GetBySupplierIdAsync(command.supplierId, cancellationToken);
			if(existing is null) {
				var entity = _mapper.Map<PartnerBankAccount>(command.Request);
				entity.SupplierId = command.supplierId;

				await _unitOfWork.PartnerBankAccounts.AddAsync(entity);
				await _unitOfWork.SaveChangesAsync(cancellationToken);

				return _mapper.Map<PartnerBankAccountDto>(entity);
			} else {
				_mapper.Map(command.Request, existing);
				await _unitOfWork.SaveChangesAsync();
				return _mapper.Map<PartnerBankAccountDto>(existing);
			}
		}
	}
}
