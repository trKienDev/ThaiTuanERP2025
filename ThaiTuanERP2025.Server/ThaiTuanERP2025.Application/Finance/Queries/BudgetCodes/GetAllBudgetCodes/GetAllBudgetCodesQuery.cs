using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllBudgetCodes
{
	public class GetAllBudgetCodesQuery : IRequest<List<BudgetCodeDto>> {}
}
