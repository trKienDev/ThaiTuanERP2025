using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Constants;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Account.Users.Commands
{
	public record SetAvatarPayload(Guid FileId);
	public sealed record SetUserAvatarCommand(SetAvatarPayload Payload) : IRequest<Unit>;
	public sealed class SetUserAvatarCommandHandler : IRequestHandler<SetUserAvatarCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;
		public SetUserAvatarCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_currentUser = currentUser;	
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(SetUserAvatarCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			var userId = _currentUser.UserId ?? throw new ValidationException("Lỗi xác thực tài khoản của bạn");

			var file = await _unitOfWork.FileAttachments.GetByIdAsync(payload.FileId)
				?? throw new NotFoundException("File không tồn tại");

			if (file.Module != ThaiTuanERPModules.Account || file.Document != AccountFileEntities.UserAvatar || file.DocumentId != userId.ToString())
				throw new ConflictException("File không thuộc về user này");

			var user = await _unitOfWork.Users.GetByIdAsync(userId)
				?? throw new NotFoundException($"Không tìm thấy thông tin user'");

			user.SetAvatar(file.Id);
			_unitOfWork.Users.Update(user);
			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
