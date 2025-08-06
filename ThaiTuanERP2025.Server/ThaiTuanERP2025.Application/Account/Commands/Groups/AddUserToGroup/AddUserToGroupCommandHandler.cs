using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.AddUserToGroup
{
	public class AddUserToGroupCommandHandler : IRequestHandler<AddUserToGroupCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AddUserToGroupCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<Unit> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
		{
			var group = await _unitOfWork.Groups.GetByIdAsync(request.GroupId) ?? throw new NotFoundException("Group không tồn tại");
			if (await _unitOfWork.UserGroups.ExistAsync(request.GroupId, request.UserId))
				throw new AppException("User đã ở trong group");

			var userGroup = new UserGroup(request.UserId, request.GroupId);
			await _unitOfWork.UserGroups.AddAsync(userGroup);
			await _unitOfWork.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
