using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.CreateLedgerAccountType
{
	public class CreateLedgerAccountTypeHandler : IRequestHandler<CreateLedgerAccountTypeCommand, LedgerAccountTypeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateLedgerAccountTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<LedgerAccountTypeDto> Handle(CreateLedgerAccountTypeCommand command, CancellationToken cancellationToken)
		{
			if (await _unitOfWork.LedgerAccountTypes.CodeExistsAsync(command.Code, null, cancellationToken))
				throw new ConflictException($"Mã loại tài khoản kế toán '{command.Code}' đã tồn tại");

			var entity = new LedgerAccountType
			{
				Code = command.Code,
				Name = command.Name,
				LedgerAccountTypeKind = command.LedgerAccountTypeKind,
				Description = command.Description
			};

			await _unitOfWork.LedgerAccountTypes.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<LedgerAccountTypeDto>(entity);
		}
	}
}
