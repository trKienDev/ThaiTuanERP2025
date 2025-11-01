using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowTemplates;
using ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowTemplates;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpenseWorkflowTemplate : AuditableEntity
	{
		#region Constructors
		private ExpenseWorkflowTemplate() { }
		public ExpenseWorkflowTemplate(string name, string documentType, int version = 1)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(documentType, nameof(documentType));
			Guard.AgainstOutOfRange(version, 1, int.MaxValue, nameof(version));

			Id = Guid.NewGuid();
			Name = name.Trim();
			DocumentType = documentType.Trim().ToLowerInvariant();
			Version = version;
			IsActive = true;

			AddDomainEvent(new ExpenseWorkflowTemplateCreatedEvent(this));
		}
		#endregion

		#region Properties
		[MaxLength(200)]
		public string Name { get; private set; } = string.Empty;

		[MaxLength(100)]
		public string DocumentType { get; private set; } = string.Empty;

		public int Version { get; private set; } = 1;
		public bool IsActive { get; private set; } = true;

		public ICollection<ExpenseStepTemplate> Steps { get; private set; } = new List<ExpenseStepTemplate>();
		#endregion

		#region Domain Behaviors

		public void Activate()
		{
			if (IsActive) return;
			if (!Steps.Any())
				throw new DomainException("Workflow template phải có ít nhất một bước phê duyệt trước khi kích hoạt.");

			IsActive = true;
			AddDomainEvent(new ExpenseWorkflowTemplateActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new ExpenseWorkflowTemplateDeactivatedEvent(this));
		}

		public void BumpVersion()
		{
			Version++;
			AddDomainEvent(new ExpenseWorkflowTemplateVersionBumpedEvent(this, Version));
		}

		public void MarkDeleted()
		{
			if (IsDeleted) return;
			IsDeleted = true;
			AddDomainEvent(new ExpenseWorkflowTemplateDeletedEvent(this));
		}

		#endregion
	}
}
