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
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.CreateTax
{
	public class CreateTaxHandler : IRequestHandler<CreateTaxCommand, TaxDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateTaxHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<TaxDto> Handle(CreateTaxCommand command, CancellationToken cancellationToken) {
			if (await _unitOfWork.Taxes.AnyAsync(x => x.PolicyName == command.PolicyName))
				throw new ConflictException($"Chính sách thuế '{command.PolicyName}' đã tồn tại");

			// ensure posting account
			var posting = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				x => x.Id == command.PostingLedgerAccountId,
				true, cancellationToken
			);
			if (posting is null) throw new NotFoundException("Tài khoản hoạch toán không tồn tại");

			// Ràng buộc ConsumptionSubType theo TaxBroadType
			var consumptionSubType = command.TaxBroadType == TaxBroadType.Consumption ? command.ConsumptionSubType : null;

			var entity = new Tax
			{
				Id = Guid.NewGuid(),
				PolicyName = command.PolicyName,
				Rate = command.Rate,
				TaxBroadType = command.TaxBroadType,
				ConsumptionSubType = command.TaxBroadType == TaxBroadType.Consumption ? command.ConsumptionSubType : null,
				PostingLedgerAccountId = command.PostingLedgerAccountId,
				Description = command.Description,
				IsActive = true
			};

			await _unitOfWork.Taxes.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(
				x => x.Id == entity.Id,
				true,
				cancellationToken, 
				x => x.PostingLedgerAccount
			);
			if (loaded is null) throw new NotFoundException();
			return _mapper.Map<TaxDto>(loaded);
		}
	}
}
