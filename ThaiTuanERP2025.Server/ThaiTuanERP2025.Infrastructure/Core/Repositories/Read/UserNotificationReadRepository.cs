using AutoMapper;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public class UserNotificationReadRepository : BaseReadRepository<UserNotification, UserNotificationDto>, IUserNotificationReadRepository
	{
		public UserNotificationReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
