using AutoMapper;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Domain.Alerts.Entities;
using ThaiTuanERP2025.Domain.Alerts.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Alerts.Repositories.Write
{
	public sealed class TaskReminderWriteRepository : BaseWriteRepository<TaskReminder>, ITaskReminderWriteRepository
	{
		public TaskReminderWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }
	}
}
