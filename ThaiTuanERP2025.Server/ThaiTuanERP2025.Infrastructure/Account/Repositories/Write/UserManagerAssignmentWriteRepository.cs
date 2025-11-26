using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories.Write
{
	public class UserManagerAssignmentWriteRepository : BaseWriteRepository<UserManagerAssignment>, IUserManagerAssignmentWriteRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public UserManagerAssignmentWriteRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
			: base(context, configurationProvider)
		{

		}
	}
}
