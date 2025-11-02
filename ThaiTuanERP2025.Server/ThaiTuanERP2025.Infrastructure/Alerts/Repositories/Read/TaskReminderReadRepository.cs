using AutoMapper;
using ThaiTuanERP2025.Application.Alerts.TaskReminders;
using ThaiTuanERP2025.Domain.Alerts.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Alerts.Repositories.Read
{
	public sealed class TaskReminderReadRepository : BaseReadRepository<TaskReminder, TaskReminderDto>, ITaskReminderReadRepository
	{
		public TaskReminderReadRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider mapperConfig)
				: base(context, mapperConfig)
		{
		}
	}
}
