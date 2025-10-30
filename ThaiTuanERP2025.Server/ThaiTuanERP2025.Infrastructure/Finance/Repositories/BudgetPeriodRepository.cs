﻿using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class BudgetPeriodRepository : BaseRepository<BudgetPeriod>, IBudgetPeriodRepository 
	{
		public BudgetPeriodRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			// Constructor logic if needed
		}
		// Implement any custom methods for BudgetPeriod here
	}
}
