using ThaiTuanERP2025.Domain.Shared.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;

namespace ThaiTuanERP2025.Domain.StoredFiles.Services
{
        public static class FileTypeRegistry
        {
                private static readonly HashSet<string> Modules = new(StringComparer.OrdinalIgnoreCase)
                {
                        ThaiTuanERPModules.Expense,
                        ThaiTuanERPModules.Finance,
                        ThaiTuanERPModules.Core,
                };

                private static readonly HashSet<string> ExpenseEntities = new(StringComparer.OrdinalIgnoreCase)
                {
                        ExpenseFileEntities.Invoice,
                        ExpenseFileEntities.PaymentAttachment,
                        ExpenseFileEntities.OutgoingAttachment,
                        ExpenseFileEntities.ExpensePaymentCommentAttachment
                };

                public static bool IsValidModule(string module) => Modules.Contains(module);

                public static bool IsValidEntity(string module, string entity)
                {
                        if (module == ThaiTuanERPModules.Expense)
                                return ExpenseEntities.Contains(entity);

                        return true; // Module khác chưa giới hạn entity
                }
        }
}
