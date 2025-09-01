using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record StepWithFlow (
		ApprovalStepInstance Step,
		ApprovalFlowInstance Flow
	);
}
