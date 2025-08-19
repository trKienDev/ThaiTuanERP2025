using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.UpdateAccountType;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.UpdateLedgerAccountType
{
	public class UpdateLedgerAccountTypeHandler : IRequestHandler<UpdateLedgerAccountTypeCommand, LedgerAccountTypeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateLedgerAccountTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<LedgerAccountTypeDto> Handle(UpdateLedgerAccountTypeCommand command, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.LedgerAccountTypes.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Không tìm thấy loại tài khoản kế toán");
			entity.Code = command.Code;
			entity.Name = command.Name;
			entity.Kind = command.Kind;
			entity.Description = command.Description;
			
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return _mapper.Map<LedgerAccountTypeDto>(entity);
		}
	}
}
