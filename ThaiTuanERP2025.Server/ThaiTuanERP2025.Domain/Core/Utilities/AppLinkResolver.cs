using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Domain.Core.Utilities
{
	public static class AppLinkResolver
	{
		public static string? Resolve(LinkType type, Guid? id)
		{
			return type switch
			{
				LinkType.BudgetPlanReview when id.HasValue => SubjectLinks.BudgetPlanDetail(id.Value),
				LinkType.BudgetPlanDetail when id.HasValue => SubjectLinks.BudgetPlanDetail(id.Value),
				LinkType.ExpensePaymentDetail when id.HasValue => SubjectLinks.ExpensePaymentApprove(id.Value),
				LinkType.Dashboard => SubjectLinks.Dashboard(),
				_ => SubjectLinks.Dashboard(),
			};
		}
	}
}
