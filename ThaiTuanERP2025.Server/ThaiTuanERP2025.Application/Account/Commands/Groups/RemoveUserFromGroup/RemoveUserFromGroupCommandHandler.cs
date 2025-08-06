using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Group.RemoveUserFromGroup
{
	public class RemoveUserFromGroupCommandHandler : IRequestHandler<RemoveUserFromGroupCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public RemoveUserFromGroupCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(RemoveUserFromGroupCommand request, CancellationToken cancellationToken)
		{
			var group = await _unitOfWork.Groups.GetByIdAsync(request.GroupId)
				?? throw new NotFoundException("Group không tồn tại");
			if (group.AdminId != request.RequestingUserId)
				throw new ForbiddenException("Chir admin mới có quyền xóa user");
			if (request.TargetUserId == group.AdminId)
				throw new AppException("Không thể tự xóa admin");

			var userGroup = await _unitOfWork.UserGroups.GetAsync(request.TargetUserId, request.GroupId)
				?? throw new NotFoundException("User không có trong group");

			await _unitOfWork.UserGroups.RemoveAsync(userGroup);
			await _unitOfWork.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
