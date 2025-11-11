using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Requests;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Users.Commands
{
	public sealed record SetUserAvatarCommand(Guid UserId, SetUserAvatarRequest Request) : IRequest<Unit>;
	public sealed class SetUserAvatarCommandHandler : IRequestHandler<SetUserAvatarCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public SetUserAvatarCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(SetUserAvatarCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var file = await _unitOfWork.StoredFiles.GetByIdAsync(request.FileId)
				?? throw new NotFoundException("File không tồn tại");

			if (file.Module != "account" || file.Entity != "user" || file.EntityId != command.UserId.ToString())
				throw new ConflictException("File không thuộc về user này");

			var user = await _unitOfWork.Users.GetByIdAsync(command.UserId)
				?? throw new NotFoundException($"Không tìm thấy thông tin user'");

			user.UpdateAvatar(request.FileId, file.ObjectKey);
			_unitOfWork.Users.Update(user);
			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
