﻿using AutoMapper;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class ExpensePaymentCommentTagRepository : BaseRepository<ExpensePaymentCommentTag>, IExpensePaymentCommentTagRepository
	{
		public ExpensePaymentCommentTagRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}
	}
}
