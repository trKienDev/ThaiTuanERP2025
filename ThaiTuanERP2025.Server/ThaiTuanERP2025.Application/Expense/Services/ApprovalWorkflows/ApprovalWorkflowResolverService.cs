using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	/// <summary>
	/// Service resolve danh sách candidate approver theo ResolverKey/Params.
	/// </summary>
	public sealed class ApprovalWorkflowResolverService
	{
		private readonly IUnitOfWork _unitOfWork;
		public ApprovalWorkflowResolverService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IReadOnlyList<Guid>> ResolveAsync( string resolverKey, string? resolverParamsJson, ExpensePayment payment, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(resolverKey))
				throw new ConflictException("ResolverKey is required.");

			var key = resolverKey.Trim().ToLowerInvariant();

			switch (key)
			{
				case "creator":
					return new[] { payment.CreatedByUserId };
				case "creator-manager":
					return await ResolveCreatorManagerAsync(payment, ct);
				case "creator-department-manager":
					return await ResolveCreatorDepartmentManagerAsync(payment, ct);
				case "user-list":
					{
						var p = Deserialize<UserListParams>(resolverParamsJson);
						return (p.UserIds ?? Array.Empty<Guid>()).Where(id => id != Guid.Empty).Distinct().ToArray();
					}

				default:
					// Bạn có thể mở rộng thêm các key khác ở đây
					throw new ConflictException($"Unsupported resolver key: {resolverKey}");
			}
		}



		// ---------------------------
		// Helpers & Param models
		// ---------------------------
		private static T Deserialize<T>(string? json) where T : new()
		{
			if (string.IsNullOrWhiteSpace(json)) return new T();
			try { 
				return JsonSerializer.Deserialize<T>(json) ?? new T();
			}
			catch { 
				return new T(); 
			}
		}

		private async Task<IReadOnlyList<Guid>> ResolveCreatorManagerAsync(ExpensePayment payment, CancellationToken cancellationToken)
		{
			// Giả định: User có ManagerId
			var creator = await _unitOfWork.Users.SingleOrDefaultIncludingAsync(u => u.Id == payment.CreatedByUserId);
			if (creator?.ManagerId is Guid m && m != Guid.Empty) 
				return new[] { m };
			
			return Array.Empty<Guid>();
		}

		private async Task<IReadOnlyList<Guid>> ResolveCreatorDepartmentManagerAsync(ExpensePayment payment, CancellationToken cancellationToken)
		{
			// Giả định: User có DepartmentId, Department có ManagerId
			var creator = await _unitOfWork.Users.SingleOrDefaultIncludingAsync(u => u.Id == payment.CreatedByUserId);
			if (creator == null) 
				return Array.Empty<Guid>();

			Guid? deptId = creator.DepartmentId; // nếu bạn dùng bảng trung gian UserDepartment, hãy điều chỉnh logic lấy dept
			if (deptId == null) 
				return Array.Empty<Guid>();

			var dept = await _unitOfWork.Departments.SingleOrDefaultIncludingAsync(d => d.Id == deptId.Value);
			if (dept?.ManagerUserId is Guid m && m != Guid.Empty) 
				return new[] { m };
			
			return Array.Empty<Guid>();
		}

		private sealed class RoleParams { public string? Role { get; set; } }

		private sealed class DepartmentRoleParams
		{
			public Guid? DepartmentId { get; set; }
			public string? Role { get; set; }
			public bool IncludeChildren { get; set; } = false;
		}

		private sealed class UserListParams { public Guid[]? UserIds { get; set; } }
	}
}
