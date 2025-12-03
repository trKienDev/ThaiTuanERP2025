using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.StoredFiles.Authorization.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Services;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Application.StoredFiles.Authorization
{
        public sealed class ExpensePaymentAttachmentPermissionChecker : IStoredFilePermissionChecker
        {
                private readonly IExpensePaymentReadRepository _expensePaymentRepo;
                public ExpensePaymentAttachmentPermissionChecker(IExpensePaymentReadRepository expensePaymentRepo)
                {
                        _expensePaymentRepo = expensePaymentRepo;
		}

		public bool CanHandle(string module, string entity)
                        => string.Equals(module, FileModules.Expense, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(entity, ExpenseFileEntities.PaymentAttachment, StringComparison.OrdinalIgnoreCase);

		public async Task<bool> HasPermissionAsync(StoredFile file, Guid userId, CancellationToken cancellationToken)
                {
                        var expensePayment = await _expensePaymentRepo.GetByAttachmentFileIdAsync(file.Id, cancellationToken);
                        if (expensePayment == null) return false;

                        return ExpensePaymentPermissionChecker.CanViewExpensePayment(expensePayment, userId);
                }
        }
}
