using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events.LedgerAccountTypes;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class LedgerAccountType : AuditableEntity
	{
		private readonly List<LedgerAccount> _ledgerAccounts = new();

		#region Constructors
		private LedgerAccountType() { } 
		public LedgerAccountType(string code, string name, LedgerAccountTypeKind kind, string? description = null)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(kind, nameof(kind));

			Id = Guid.NewGuid();
			Code = code.Trim().ToLowerInvariant();
			Name = name.Trim();
			Kind = kind;
			Description = description?.Trim();
			IsActive = true;

			AddDomainEvent(new LedgerAccountTypeCreatedEvent(this));
		}
		#endregion

		#region Properties
		public string Code { get; private set; } = null!;
		public string Name { get; private set; } = null!;
		public LedgerAccountTypeKind Kind { get; private set; }
		public string? Description { get; private set; }
		public bool IsActive { get; private set; }

		public IReadOnlyCollection<LedgerAccount> LedgerAccounts => _ledgerAccounts.AsReadOnly();
		#endregion

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new LedgerAccountTypeRenamedEvent(this));
		}

		public void ChangeDescription(string? newDesc)
		{
			Description = newDesc?.Trim();
			AddDomainEvent(new LedgerAccountTypeUpdatedEvent(this));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new LedgerAccountTypeActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new LedgerAccountTypeDeactivatedEvent(this));
		}
		#endregion
	}
}
