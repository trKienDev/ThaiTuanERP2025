using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Partner.Entities;

namespace ThaiTuanERP2025.Application.Partner.Commands.Suppliers.CreateSupplier
{
	public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICodeGenerator _codeGenerator;
		public CreateSupplierCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICodeGenerator codeGenerator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_codeGenerator = codeGenerator;
		}

		public async Task<SupplierDto> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
		{
			// nếu user nhập code -> chuẩn hóa + check trùng
			string? code = command.Request.Code?.Trim();
			if (!string.IsNullOrEmpty(code))
			{
				code = code.ToUpperInvariant();
				if(await _unitOfWork.Suppliers.ExistsByCodeAsync(code, cancellationToken))
				{
					throw new ConflictException($"Mã nhà cung cấp '{code}' đã tồn tại.");
				}
			} else {
				// không nhập -> tự sinh theo prefix truyền vào
				var key = "Supplier";
				code = await _codeGenerator.NextAsync(key, prefix: "SUP-", padLength: 6, start: 1, cancellationToken);
				// double-check cực hiếm nếu ai đó vô tình dùng đúng chuỗi này
				if (await _unitOfWork.Suppliers.ExistsByCodeAsync(code, cancellationToken))
					code = await _codeGenerator.NextAsync(key, prefix: "SUP-", padLength: 6, start: 1, cancellationToken);
			}

			var entity = _mapper.Map < Supplier>(command.Request);
			entity.Code = code;
			entity.DefaultCurrency = entity.DefaultCurrency.ToUpperInvariant();

			await _unitOfWork.Suppliers.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<SupplierDto>(entity);
		}
	}
}
