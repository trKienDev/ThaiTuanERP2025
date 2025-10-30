using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class UserManagerAssignmentRepository : BaseRepository<UserManagerAssignment>, IUserManagerAssignmentRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public UserManagerAssignmentRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
			: base(context, configurationProvider)
		{

		}
	}
}
