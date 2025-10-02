using System.Text.Json;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	internal static class StepInstanceExtensions
	{
		public static IReadOnlyCollection<Guid> GetResolvedApproverCandidates(this ApprovalStepInstance step)
		{
			if (string.IsNullOrWhiteSpace(step.ResolvedApproverCandidatesJson))
				return Array.Empty<Guid>();

			try
			{
				var ids = JsonSerializer.Deserialize<List<Guid>>(step.ResolvedApproverCandidatesJson)
					  ?? new List<Guid>();
				return ids;
			}
			catch
			{
				return Array.Empty<Guid>();
			}
		}
	}
}
