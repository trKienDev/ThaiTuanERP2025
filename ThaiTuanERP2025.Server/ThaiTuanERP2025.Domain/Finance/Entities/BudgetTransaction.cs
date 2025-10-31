using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events.BudgetTransactions;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	/// <summary>
	/// Ghi nhận bút toán thay đổi ngân sách (ledger entry) cho từng BudgetPlan.
	/// </summary>
	public class BudgetTransaction : AuditableEntity
	{
		private BudgetTransaction() { } // EF only

		public BudgetTransaction(Guid budgetPlanId, Guid paymentId, decimal amount, BudgetTransactionType type)
		{
			Guard.AgainstDefault(budgetPlanId, nameof(budgetPlanId));
			Guard.AgainstDefault(paymentId, nameof(paymentId));
			Guard.AgainstZeroOrNegative(amount, nameof(amount));
			Guard.AgainstInvalidEnumValue(type, nameof(type));

			Id = Guid.NewGuid();
			BudgetPlanId = budgetPlanId;
			PaymentId = paymentId;
			Amount = amount;
			Type = type;
			TransactionDate = DateTime.UtcNow;

			AddDomainEvent(new BudgetTransactionCreatedEvent(this));
		}

		public Guid BudgetPlanId { get; private set; }
		public Guid PaymentId { get; private set; }
		public decimal Amount { get; private set; }
		public BudgetTransactionType Type { get; private set; }
		public DateTime TransactionDate { get; private set; }

		public BudgetPlan BudgetPlan { get; private set; } = null!;

		#region Domain Behaviors

		public void Reverse()
		{
			AddDomainEvent(new BudgetTransactionReversedEvent(this));
		}

		public void AdjustAmount(decimal newAmount)
		{
			Guard.AgainstZeroOrNegative(newAmount, nameof(newAmount));
			Amount = newAmount;
			AddDomainEvent(new BudgetTransactionAdjustedEvent(this));
		}

		#endregion
	}
}
