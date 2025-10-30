using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalWorkflowTemplate : AuditableEntity
	{
		private ApprovalWorkflowTemplate() { }

		public ApprovalWorkflowTemplate(string name, string documentType, int version = 1)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(documentType, nameof(documentType));
			Guard.AgainstOutOfRange(version, 1, int.MaxValue, nameof(version));

			Id = Guid.NewGuid();
			Name = name.Trim();
			DocumentType = documentType.Trim().ToLowerInvariant();
			Version = version;
			IsActive = true;

			AddDomainEvent(new ApprovalWorkflowTemplateCreatedEvent(this));
		}

		[MaxLength(200)]
		public string Name { get; private set; } = string.Empty;

		[MaxLength(100)]
		public string DocumentType { get; private set; } = string.Empty;

		public int Version { get; private set; } = 1;
		public bool IsActive { get; private set; } = true;

		public ICollection<ApprovalStepTemplate> Steps { get; private set; } = new List<ApprovalStepTemplate>();

		#region Domain Behaviors

		public void Activate()
		{
			if (IsActive) return;
			if (!Steps.Any())
				throw new DomainException("Workflow template phải có ít nhất một bước phê duyệt trước khi kích hoạt.");

			IsActive = true;
			AddDomainEvent(new ApprovalWorkflowTemplateActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new ApprovalWorkflowTemplateDeactivatedEvent(this));
		}

		public void BumpVersion()
		{
			Version++;
			AddDomainEvent(new ApprovalWorkflowTemplateVersionBumpedEvent(this, Version));
		}

		public void MarkDeleted()
		{
			if (IsDeleted) return;
			IsDeleted = true;
			AddDomainEvent(new ApprovalWorkflowTemplateDeletedEvent(this));
		}

		#endregion
	}
}
