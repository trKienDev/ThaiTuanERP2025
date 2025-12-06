using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public sealed class Comment : AuditableEntity
	{
		#region Constructor
		private Comment() { }
		public Comment(string module, string entity, Guid entityId, Guid userId, string content)
		{
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(entity, nameof(entity));
			Guard.AgainstDefault(entityId, nameof(entityId));
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstNullOrWhiteSpace(content, nameof(content));

			Id = Guid.NewGuid();
			Module = module.Trim();
			Entity = entity.Trim();
			EntityId = entityId;
			UserId = userId;
			Content = content.Trim();
		}
		#endregion

		#region Properties
		public string Module { get; private set; } = string.Empty;   // vd: "Finance"
		public string Entity { get; private set; } = string.Empty;   // vd: "ExpensePayment"
		public Guid EntityId { get; private set; }                   // Id của document

		public Guid UserId { get; private set; }
		public User User { get; set; } = default!;

		public string Content { get; private set; } = string.Empty;
		#endregion

		public void UpdateContent(string content)
		{
			Guard.AgainstNullOrWhiteSpace(content, nameof(content));
			Content = content.Trim();
		}
	}
}
