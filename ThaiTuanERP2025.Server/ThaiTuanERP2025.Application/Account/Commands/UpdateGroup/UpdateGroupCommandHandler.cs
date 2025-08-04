using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.UpdateGroup
{
	public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public UpdateGroupCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
		{
			var group = await _unitOfWork.Groups.GetByIdAsync(request.GroupId)
				?? throw new NotFoundException("Group không tồn tại");
			if (group.AdminId != request.RequestingUserId)
				throw new ForbiddenException("Chỉ admin mới có quyền cập nhật thông tin nhóm");
			
			group.Rename(request.NewName);
			group.UpdateDescription(request.NewDescription);

			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
