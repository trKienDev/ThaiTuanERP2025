using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Services.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Services
{
	public class ApproverResolverService : IApproverResolverService
	{
		public IReadOnlyList<Guid> Resolve(ApprovalStepDefinition stepDefinition, Guid? selectedApproverId)
		{
			switch (stepDefinition.ResolverType)
			{
				case ApprovalResolverType.FixedUser:
					var fixedUser = JsonSerializer.Deserialize<FixedUserParam>(stepDefinition.ResolverParamsJson) 
						?? throw new InvalidOperationException($"Invalid params for step {stepDefinition.Name}");
					return new[] { fixedUser.UserId };
				case ApprovalResolverType.UserPickFromList:
					var al = JsonSerializer.Deserialize<UserListParam>(stepDefinition.ResolverParamsJson) 
						?? throw new InvalidOperationException($"Invalid params for step {stepDefinition.Name}");
					if (selectedApproverId is null || !al.UserIds.Contains(selectedApproverId.Value))
						throw new InvalidOperationException($"Selected approver not allowed for step {stepDefinition.Name}");
					return new[] { selectedApproverId.Value };
				case ApprovalResolverType.UserList:
					var ul = JsonSerializer.Deserialize<UserListParam>(stepDefinition.ResolverParamsJson)
						 ?? throw new InvalidOperationException($"Invalid params for step {stepDefinition.Name}");
					if (ul.UserIds is null || ul.UserIds.Count == 0)
						throw new InvalidOperationException($"Step {stepDefinition.Name} has empty candidate list");
					return ul.UserIds;
				default: 
					throw new NotSupportedException($"ResolverType {stepDefinition.ResolverType} not supported");
			}
		}

		private sealed record FixedUserParam(Guid UserId);
		private sealed record UserListParam(List<Guid> UserIds);
	}
}
