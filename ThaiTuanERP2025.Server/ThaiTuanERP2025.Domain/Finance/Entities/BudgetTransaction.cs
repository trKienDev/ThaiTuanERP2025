using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events.BudgetTransactions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	/// <summary>
	/// Ghi nhận bút toán thay đổi ngân sách (ledger entry) cho từng BudgetPlan.
	/// </summary>
	public class BudgetTransaction : BaseEntity
	{
		#region Constructors
		private BudgetTransaction() { } 
		public BudgetTransaction(Guid planDetailId, Guid paymentItemId, decimal amount, BudgetTransactionType type)
		{
			Guard.AgainstDefault(planDetailId, nameof(planDetailId));
			Guard.AgainstDefault(paymentItemId, nameof(paymentItemId));
			Guard.AgainstZeroOrNegative(amount, nameof(amount));
			Guard.AgainstInvalidEnumValue(type, nameof(type));

			Id = Guid.NewGuid();
			BudgetPlanDetailId = planDetailId;
			ExpensePaymentItemId = paymentItemId;
			Amount = amount;
			Type = type;
			TransactionDate = DateTime.UtcNow;

			AddDomainEvent(new BudgetTransactionCreatedEvent(this));
		}
		#endregion

		#region Properties
		public Guid BudgetPlanDetailId { get; private set; }
		public BudgetPlan BudgetPlanDetail { get; private set; } = null!;

		public Guid ExpensePaymentItemId { get; private set; }
		public ExpensePaymentItem ExpensePaymentItem { get; private set; } = null!;

		public decimal Amount { get; private set; }
		public BudgetTransactionType Type { get; private set; }
		public DateTime TransactionDate { get; private set; }

		#endregion

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
