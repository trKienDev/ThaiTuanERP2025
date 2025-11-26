using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
	public sealed record ExpensePaymentPayload
	{
		public string Name { get; init; } = default!;
		public string? Description { get; init; } = string.Empty;
		public PayeeType PayeeType { get; init; } 
		public Guid? SupplierId { get; init; }
		public Guid ManagerApproverId { get; init; }

		public string BankName { get; init; } = default!;
		public string AccountNumber { get; init; } = default!;
		public string BeneficiaryName { get; init; } = default!;

		public DateTime DueDate { get; init; }
		public bool hasGoodsReceipt { get; init; }

		public List<ExpensePaymentItemPayload> Items { get; init; } = new List<ExpensePaymentItemPayload>();
		public List<Guid> followerIds { get; init; } = new List<Guid>();
	}
}
