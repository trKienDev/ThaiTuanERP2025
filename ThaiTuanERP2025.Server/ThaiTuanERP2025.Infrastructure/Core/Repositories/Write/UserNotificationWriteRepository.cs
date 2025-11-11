using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Write
{
	public class UserNotificationWriteRepository : BaseWriteRepository<UserNotification>, IUserNotificationWriteRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public UserNotificationWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
	}
}
