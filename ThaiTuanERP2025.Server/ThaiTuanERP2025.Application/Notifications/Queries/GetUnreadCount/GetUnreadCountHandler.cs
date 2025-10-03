using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Notifications.Queries.GetUnreadCount
{
	public sealed class GetUnreadCountHandler : IRequestHandler<GetUnreadCountQuery, int>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public GetUnreadCountHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;
			return await _unitOfWork.Notifications.CountAsync(
				n => n.UserId == uid && !n.IsRead,
				cancellationToken
			);
		}
	}
}
