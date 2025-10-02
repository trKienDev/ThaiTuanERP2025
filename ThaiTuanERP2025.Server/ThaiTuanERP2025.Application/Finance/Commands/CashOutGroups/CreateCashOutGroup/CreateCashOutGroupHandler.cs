using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Common.Utils;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.CreateCashoutGroup
{
	public class CreateCashoutGroupHandler : IRequestHandler<CreateCashoutGroupCommand, CashoutGroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateCashoutGroupHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<CashoutGroupDto> Handle(CreateCashoutGroupCommand request, CancellationToken cancellationToken) {		
			var name = request.Name?.Trim();
			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Name", "Tên là bắt buộc");

			// Tự sinh code nếu không có
			var code = string.IsNullOrWhiteSpace(request.Code) ? CodeGenerator.FromName(name) : request.Code.Trim();

			// Kiểm tra trùng code trong cùng Parent
			var exists = await _unitOfWork.CashoutGroups.AnyAsync(g => g.ParentId == request.ParentId && g.Code == code);
			if (exists)
				throw new ConflictException($"Mã dòng tiền ra '{code}' đã tồn tại");

			// Parent must exists
			CashoutGroup? parent = null;
			if (request.ParentId.HasValue) { 
				parent = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(g => g.Id == request.ParentId.Value);
				if (parent is null)
					throw new NotFoundException("Không tìm thấy CashoutGroup cha");
			}

			var entity = new CashoutGroup
			{
				Code = code,
				Name = name,
				Description = request.Description?.Trim(),
				IsActive = true,
				ParentId = request.ParentId,
				Level = parent?.Level + 1 ?? 0,
				Path = (parent?.Path ?? "/") + code + "/"
			};

			await _unitOfWork.CashoutGroups.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			var loaded = await _unitOfWork.CashoutGroups.SingleOrDefaultAsync(q => q.Where(x => x.Id == entity.Id));
			if (loaded is null) throw new NotFoundException($"Có lỗi trong quá trình xử lý. Không tìm thấy mã dòng tiền ra đã được tạo");
			return _mapper.Map<CashoutGroupDto>( loaded );
		}
	}
}
