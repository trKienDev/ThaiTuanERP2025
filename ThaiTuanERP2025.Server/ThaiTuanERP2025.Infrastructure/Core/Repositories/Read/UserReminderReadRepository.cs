using AutoMapper;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Application.Core.Reminders.Contracts;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public class UserReminderReadRepository : BaseReadRepository<UserReminder, UserReminderDto>, IUserReminderReadRepository
	{
		public UserReminderReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
