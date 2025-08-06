using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository 
	{
		public BankAccountRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext)
		{
		}
		// You can add custom methods for BankAccount here if needed
	}
}
