using AutoMapper;
using ThaiTuanERP2025.Domain.Notifications.Repositories;
using ThaiTuanERP2025.Domain.Notifications.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Notifications.Repositories
{
	public sealed class TaskReminderRepository : BaseRepository<TaskReminder>, ITaskReminderRepository
	{
		public TaskReminderRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }
	}
}
