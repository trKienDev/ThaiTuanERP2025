using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.SetUserManagers
{
	public sealed class SetUserManagersHandler : IRequestHandler<SetUserManagersCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public SetUserManagersHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(SetUserManagersCommand request, CancellationToken cancellationToken)
		{
			// 1 ) User có tồn tại
			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
			if (user is null)
				throw new NotFoundException("Người dùng không tồn tại");

			// 2 ) Chuẩn bị dữ liệu
			var desired = request.ManagerIds.Distinct().Where(id => id != request.UserId).ToList();
			
			// 3 ) Lấy assignments hiện còn hiệu lực
			var current = await _unitOfWork.Users.GetActiveManagerAssignmentsAsync(request.UserId);
			var currentIds = current.Select(x => x.ManagerId).ToHashSet();

			// 4 ) Thu hồi các quan hệ không còn (đặt RevokeAt)
			foreach (var a in current.Where(x => !desired.Contains(x.ManagerId)))
				a.RevokedAt = DateTime.UtcNow;

			// 5 ) Thêm mới các quan hệ còn thiếu
			var toAdd = desired.Where(id => !currentIds.Contains(id))
							.Select(mid => new UserManagerAssignment{
								UserId = request.UserId,
								ManagerId = mid,
								IsPrimary = false,
								AssignedAt = DateTime.UtcNow,
								RevokedAt = null
							}).ToList();
			if (toAdd.Count > 0)
				await _unitOfWork.Users.AddAssignmentsAsync(toAdd);

			// 6 ) Đặt Primary 
			var primaryId = request.PrimaryManagerId ?? desired.FirstOrDefault();
			foreach (var a in current.Where(x => x.RevokedAt == null))
				a.IsPrimary = false;

			if(primaryId != Guid.Empty && desired.Contains(primaryId)) {
				var hit = current.FirstOrDefault(x => x.ManagerId == primaryId && x.RevokedAt == null)
					?? toAdd.FirstOrDefault(x => x.ManagerId == primaryId);
				if (hit is not null)
					hit.IsPrimary = true;
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
