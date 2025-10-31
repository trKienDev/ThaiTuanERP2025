using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class LedgerAccountType : AuditableEntity
	{
		private readonly List<LedgerAccount> _ledgerAccounts = new();

		private LedgerAccountType() { } // EF only

		public LedgerAccountType(string code, string name, LedgerAccountTypeKind kind, string? description = null)
		{
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(kind, nameof(kind));

			Id = Guid.NewGuid();
			Code = code.Trim().ToUpperInvariant();
			Name = name.Trim();
			LedgerAccountTypeKind = kind;
			Description = description?.Trim();
			IsActive = true;

			AddDomainEvent(new LedgerAccountTypeCreatedEvent(this));
		}

		public string Code { get; private set; } = null!;
		public string Name { get; private set; } = null!;
		public LedgerAccountTypeKind LedgerAccountTypeKind { get; private set; }
		public string? Description { get; private set; }
		public bool IsActive { get; private set; }

		public IReadOnlyCollection<LedgerAccount> LedgerAccounts => _ledgerAccounts.AsReadOnly();

		public User CreatedByUser { get; private set; } = null!;
		public User? ModifiedByUser { get; private set; }
		public User? DeletedByUser { get; private set; }

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
