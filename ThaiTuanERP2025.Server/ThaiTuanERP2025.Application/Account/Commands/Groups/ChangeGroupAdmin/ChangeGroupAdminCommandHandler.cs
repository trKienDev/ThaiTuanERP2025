using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Group.ChangeGroupAdmin
{
	public class ChangeGroupAdminCommandHandler : IRequestHandler<ChangeGroupAdminCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ChangeGroupAdminCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(ChangeGroupAdminCommand request, CancellationToken cancellationToken)
		{
			var group = await _unitOfWork.Groups.GetByIdAsync(request.GroupId)
				?? throw new NotFoundException($"Group không tồn tại.");
			if (group.AdminId != request.RequestorId)
				throw new ForbiddenException("Chỉ admin mới có quyền chuyển admin");

			var newAdminExists = await _unitOfWork.UserGroups.ExistAsync(request.TargetUserId, request.GroupId);
			if (!newAdminExists)
				throw new NotFoundException($"Người được chọn làm admin không nằm trong group.");

			group.SetAdmin(request.TargetUserId);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
