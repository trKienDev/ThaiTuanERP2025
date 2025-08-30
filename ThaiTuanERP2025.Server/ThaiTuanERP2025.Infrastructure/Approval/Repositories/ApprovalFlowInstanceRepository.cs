using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Approval.Repositories
{
	public class ApprovalFlowInstanceRepository : BaseRepository<ApprovalFlowInstance>, IApprovalFlowInstanceRepository 
	{
		public ApprovalFlowInstanceRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapper) 
			: base(dbContext, mapper) { }
	}
}
