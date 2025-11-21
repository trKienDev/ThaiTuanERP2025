using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Finance.Events.CashoutGroups;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class CashoutGroup : AuditableEntity, IActiveEntity
	{
		#region Constructor
		private CashoutGroup() { }
		public CashoutGroup(string name, Guid? parentId = null, string ? description = null)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));

			Id = Guid.NewGuid();
			Name = name.Trim();
			Description = description?.Trim();
			ParentId = parentId;
			OrderNumber = 0;
			Path = string.Empty;

			AddDomainEvent(new CashoutGroupCreatedEvent(this));
		}
		#endregion

		#region Properties
		public string Name { get; private set; } = null!;
		public string? Description { get; private set; }
		public bool IsActive { get; private set; } = true;

		public Guid? ParentId { get; private set; }
		public CashoutGroup? Parent { get; init; }
		public ICollection<CashoutGroup> Children { get; private set; } = new List<CashoutGroup>();

		public ICollection<CashoutCode> CashoutCodes { get; private set; } = new List<CashoutCode>();

		public int Level { get; private set; }
		public int OrderNumber { get; private set; }
		public string Path { get; private set; } = "";

		#endregion

		#region Domain Behaviors

		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new CashoutGroupRenamedEvent(this));
		}

		public void SetParent(CashoutGroup? parent, int newOrderNumber)
		{
			Level = parent == null ? 0 : parent.Level + 1;

			OrderNumber = newOrderNumber;

			Path = parent == null ? $"/{OrderNumber}" : $"{parent.Path}/{OrderNumber}";

			AddDomainEvent(new CashoutGroupHierarchyChangedEvent(this));
		}

		public void AddChild(CashoutGroup child)
		{
			Guard.AgainstNull(child, nameof(child));
			if (Children.Any(c => c.Id == child.Id))
				return;
			Children.Add(child);
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new CashoutGroupActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new CashoutGroupDeactivatedEvent(this));
		}

		#endregion
	}
}
