using ThaiTuanERP2025.Application.Files.Authorization.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Services;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;

namespace ThaiTuanERP2025.Application.StoredFile.Authorization
{
        public sealed class ExpensePaymentAttachmentPermissionChecker : IStoredFilePermissionChecker
        {
                public bool CanHandle(string module, string entity)
                    => module == FileModules.Expense
                    && entity == ExpenseFileEntities.PaymentAttachment;

                public async Task<bool> HasPermissionAsync(StoredFile file, Guid userId, CancellationToken ct)
                {
                        var ep = await _expensePaymentRepo.GetByAttachmentFileIdAsync(file.Id, ct);
                        if (ep == null) return false;

                        return ExpensePaymentPermissionChecker.CanViewExpensePayment(ep, userId);
                }
        }
}
