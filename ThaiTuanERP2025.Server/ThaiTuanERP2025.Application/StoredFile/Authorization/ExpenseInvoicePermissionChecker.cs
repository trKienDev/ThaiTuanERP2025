using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Files.Authorization.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Services;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;

namespace ThaiTuanERP2025.Application.Files.Authorization
{
	public sealed class ExpenseInvoicePermissionChecker : IStoredFilePermissionChecker
	{
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		public ExpenseInvoicePermissionChecker(IExpensePaymentReadRepository expensePaymentRepo)
		{
			_expensePaymentRepo = expensePaymentRepo;
		}

		public bool CanHandle(string module, string entity) => module.Equals(FileModules.Expense, StringComparison.OrdinalIgnoreCase) && entity.Equals(ExpenseFileEntities.Invoice, StringComparison.OrdinalIgnoreCase);

		public async Task<bool> HasPermissionAsync(StoredFile file, Guid userId, CancellationToken ct)
		{
			// Tìm ExpensePayment chứa file invoice này
			var ep = await _expensePaymentRepo.GetByInvoiceFileIdAsync(file.Id, ct);
			if (ep == null) return false;

			return ExpensePaymentPermissionChecker.CanViewExpensePayment(ep, userId);
		}
	}
}
