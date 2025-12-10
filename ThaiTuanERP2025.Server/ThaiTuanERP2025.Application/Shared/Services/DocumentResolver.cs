using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Shared.Services
{
	public interface IDocumentResolver
	{
		Task<string> GetDocumentNameAsync(DocumentType type, Guid documentId, CancellationToken cancellationToken);
		Task<Guid?> GetDocumentCreatorIdAsync(DocumentType type, Guid documentId, CancellationToken cancellationToken);
	}

	public sealed class DocumentResolver : IDocumentResolver
	{
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		public DocumentResolver(IExpensePaymentReadRepository expensePaymentRepo)
		{
			_expensePaymentRepo = expensePaymentRepo;
		}

		public async Task<string> GetDocumentNameAsync(DocumentType type, Guid documentId, CancellationToken cancellationToken)
		{
			return type switch
			{
				DocumentType.ExpensePayment => (await _expensePaymentRepo.GetNameAsync(documentId, cancellationToken)) ?? "Phiếu thanh toán",
				_ => "Chứng từ"
			};
		}

		public async Task<Guid?> GetDocumentCreatorIdAsync(DocumentType type, Guid documentId, CancellationToken cancellationToken)
		{
			return type switch
			{
				DocumentType.ExpensePayment => (await _expensePaymentRepo.GetCreatorIdAsync(documentId, cancellationToken)),

				_ => throw new NotSupportedException($"DocumentType '{type}' không khả dụng.")
			};
		}
	}
}
