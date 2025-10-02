using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.CashoutOutGroup;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Common.Utils;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.UpdateCashoutGroup
{
	public class UpdateCashoutGroupHandler : IRequestHandler<UpdateCashoutGroupCommand, CashoutGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateCashoutGroupHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashoutGroupDto> Handle(UpdateCashoutGroupCommand request, CancellationToken cancellationToken) {
			var name = request.Name.Trim();
			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Name", "Tên là bắt buộc");

			var code = string.IsNullOrWhiteSpace(request.Code) ? CodeGenerator.FromName(name) : request.Code.Trim();

			var entity = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(x => x.Id == request.Id, asNoTracking: false);
			if (entity is null)
				throw new NotFoundException("Không tìm thấy nhóm dòng tiền");

			// Kiểm tra trùng mã nếu mã mới khác mã cũ
			if (!string.Equals(entity.Code, code, StringComparison.OrdinalIgnoreCase))
			{
				var exists = await _unitOfWork.CashoutGroups.AnyAsync(x => x.Code == code && x.Id != entity.Id);
				if (exists)
					throw new ConflictException($"Mã dòng tiền ra '{code}' đã tồn tại.");
			}

			entity.Code = code;	
			entity.Name = name;
			entity.Description = request.Description?.Trim();

			// Cập nhật Path nếu đổi code
			if (entity.ParentId == null)
			{
				entity.Path = "/" + code + "/";
			}
			else
			{
				var parent = await _unitOfWork.CashoutGroups
					.SingleOrDefaultAsync(q => q.Where(g => g.Id == entity.ParentId));
				entity.Path = (parent?.Path ?? "/") + code + "/";
			}


			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var loaded = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(x =>x.Id == entity.Id);
			if (loaded is null) throw new NotFoundException();
			return _mapper.Map<CashoutGroupDto>(loaded);
		}
	}
}
