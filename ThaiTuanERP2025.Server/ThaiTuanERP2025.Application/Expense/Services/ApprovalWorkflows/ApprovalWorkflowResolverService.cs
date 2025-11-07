using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
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
				{
					var id = payment.CreatedByUserId;
					return (id.HasValue && id.Value != Guid.Empty)
						? new[] { id.Value }
						: Array.Empty<Guid>();
				}
				case "creator-manager":
					return await ResolveCreatorManagerAsync(payment, ct);
				case "manager-department":
					return await ResolveManagerDepartmentAsync(payment, ct);
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

		private async Task<IReadOnlyList<Guid>> ResolveManagerDepartmentAsync(ExpensePayment payment, CancellationToken cancellationToken)
		{
			// Ưu tiên ManagerApproverId gắn trực tiếp trong phiếu
			if (payment.ManagerApproverId != Guid.Empty)
				return new[] { payment.ManagerApproverId };

			// Fallback: lấy manager theo Department của creator
			var creator = await _unitOfWork.Users.SingleOrDefaultIncludingAsync(u => u.Id == payment.CreatedByUserId);
			if (creator == null)
				return Array.Empty<Guid>();

			if (creator?.DepartmentId is not Guid deptId)
				return Array.Empty<Guid>();

			var dept = await _unitOfWork.Departments.SingleOrDefaultIncludingAsync(
				d => d.Id == deptId,
				asNoTracking: true,
				cancellationToken: cancellationToken,
				d => d.Managers
			);
			if (dept is null || !dept.Managers.Any())
				return Array.Empty<Guid>();

			// 3) Ưu tiên primary manager
			var primary = dept.Managers.FirstOrDefault(m => m.IsPrimary)?.UserId;
			if (primary.HasValue && primary.Value != Guid.Empty)
				return new[] { primary.Value };

			// 4) Nếu không có primary, trả về tất cả manager của phòng ban
			return dept.Managers.Select(m => m.UserId).Distinct().ToArray();
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
