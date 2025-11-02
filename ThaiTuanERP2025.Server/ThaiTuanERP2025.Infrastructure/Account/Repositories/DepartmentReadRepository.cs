using AutoMapper;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class DepartmentReadRepository : BaseReadRepository<Department, DepartmentDto>
	{
		public DepartmentReadRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig) { }
	}
}
