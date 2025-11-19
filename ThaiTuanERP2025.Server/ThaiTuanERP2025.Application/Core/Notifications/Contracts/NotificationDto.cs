using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Notifications.Contracts
{
	public sealed record UserNotificationDto
	{
		public Guid Id { get; init; }

		public Guid SenderId { get; init; }
		public UserBriefAvatarDto Sender { get; init; } = default!;


		public string Title { get; init; } = default!;
		public string Message { get; init; } = default!;

		/// <summary>Đường dẫn Angular routing</summary>
		public string? Link { get; init; }

		/// <summary>Loại link để UI có thể xử lý tùy theo loại thông báo</summary>
		public LinkType LinkType { get; init; }

		/// <summary>ID của đối tượng liên quan (BudgetPlanId, ExpenseId, RequestId)</summary>
		public Guid? TargetId { get; init; }

		/// <summary>Loại thông báo: Info / Warning / Task / Approval / System...</summary>
		public NotificationType Type { get; init; }

		public DateTime CreatedAt { get; init; }
		public bool IsRead { get; init; }
	}
}
