using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetMyPendingApproval
{
	public record GetMyPendingApprovalQuery : IRequest<IReadOnlyList<MyPendingApprovalDto>>;
}
