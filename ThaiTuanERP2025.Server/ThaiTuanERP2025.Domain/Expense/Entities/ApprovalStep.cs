using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalStep : BaseEntity
	{
		public string Name { get; private set; } = string.Empty;
		public int SlaHours { get; private set; }
		public FlowType FlowType { get; private set; }
		public int Order { get; private set; }

		public List<Guid> ApproverIds { get; private set; } = new List<Guid>();
		private ApprovalStep() { }

		public ApprovalStep(string name, int slaHours, FlowType flowType, List<Guid> approverIds, int order)
		{
			Name = name;
			SlaHours = slaHours;
			FlowType = flowType;
			ApproverIds = approverIds;
			Order = order;
		}
		public void UpdateOrder(int newOrder) {
			Order = newOrder;
		}
	}
}
