using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.StoredFiles.Authorization.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Services;
using ThaiTuanERP2025.Domain.Shared.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Application.Files.Authorization
{
	public sealed class ExpenseInvoicePermissionChecker : IStoredFilePermissionChecker
	{
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		public ExpenseInvoicePermissionChecker(IExpensePaymentReadRepository expensePaymentRepo)
		{
			_expensePaymentRepo = expensePaymentRepo;
		}

		public bool CanHandle(string module, string entity) 
			=> module.Equals(ThaiTuanERPModules.Expense, StringComparison.OrdinalIgnoreCase) 
			&& entity.Equals(ExpenseFileEntities.Invoice, StringComparison.OrdinalIgnoreCase);

		public async Task<bool> HasPermissionAsync(StoredFile file, Guid userId, CancellationToken cancellationToken)
		{
			// Tìm ExpensePayment chứa file invoice này
			var ep = await _expensePaymentRepo.GetByInvoiceFileIdAsync(file.Id, cancellationToken);
			if (ep == null) return false;

			return ExpensePaymentPermissionChecker.CanViewExpensePayment(ep, userId);
		}
	}
}
