using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatarFileId
{
	public class UpdateUserAvatarFileIdHandler : IRequestHandler<UpdateUserAvatarFileIdCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateUserAvatarFileIdHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), "Unit of Work cannot be null");
		}

		public async Task<Unit> Handle(UpdateUserAvatarFileIdCommand request, CancellationToken cancellationToken)
		{
			var file = await _unitOfWork.StoredFiles.GetByIdAsync(request.FileId)
				?? throw new NotFoundException("File không tồn tại");

			// xác thực đúng module/entity cho chắc
			if (file.Module != "account" || file.Entity != "user" || file.EntityId != request.UserId.ToString())
				throw new ConflictException("File không thuộc về user này");

			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId)
				?? throw new NotFoundException($"Không tìm thấy thông tin user'");

			user.UpdateAvatar(request.FileId);
			_unitOfWork.Users.Update(user);
			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
