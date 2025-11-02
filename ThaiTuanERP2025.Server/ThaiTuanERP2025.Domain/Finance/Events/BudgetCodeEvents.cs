using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public abstract class BudgetCodeEventBase : IDomainEvent
	{
		public Guid BudgetCodeId { get; }
		public DateTime OccurredOn { get; }

		protected BudgetCodeEventBase(Guid codeId)
		{
			BudgetCodeId = codeId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetCodeActivatedEvent : BudgetCodeEventBase
	{
		public BudgetCodeActivatedEvent(BudgetCode code) : base(code.Id)
		{
			BudgetCode = code;
		}

		public BudgetCode BudgetCode { get; }
	}

	public sealed class BudgetCodeCodeChangedEvent : BudgetCodeEventBase
	{
		public BudgetCodeCodeChangedEvent(BudgetCode budgetCode) : base(budgetCode.Id) 
		{
			BudgetCode = budgetCode;
		}

		public BudgetCode BudgetCode { get; }
	}

	public sealed class BudgetCodeCreatedEvent : BudgetCodeEventBase
	{
		public BudgetCodeCreatedEvent(BudgetCode budgetCode) : base(budgetCode.Id)
		{
			BudgetCode = budgetCode;
		}
		public BudgetCode BudgetCode { get; }
	}

	public sealed class BudgetCodeDeactivatedEvent : BudgetCodeEventBase
	{
		public BudgetCodeDeactivatedEvent(BudgetCode budgetCode) : base (budgetCode.Id) 
		{
			BudgetCode = budgetCode;
		}
		public BudgetCode BudgetCode { get; }
	}

	public sealed class BudgetCodeGroupChangedEvent : BudgetCodeEventBase
	{
		public BudgetCodeGroupChangedEvent(BudgetCode budgetCode) : base(budgetCode.Id)
		{
			BudgetCode = budgetCode;
		}
		public BudgetCode BudgetCode { get; }
	}

	public sealed class BudgetCodeRenamedEvent : BudgetCodeEventBase
	{
		public BudgetCodeRenamedEvent(BudgetCode budgetCode) : base(budgetCode.Id)
		{
			BudgetCode = budgetCode;
		}

		public BudgetCode BudgetCode { get; }
	}

}
