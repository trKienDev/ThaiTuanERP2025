using ThaiTuanERP2025.Domain.Shared.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;

namespace ThaiTuanERP2025.Domain.StoredFiles.Services
{
        public static class FileTypeRegistry
        {
                private static readonly HashSet<string> Modules = new(StringComparer.OrdinalIgnoreCase)
                {
                        ThaiTuanERPModules.Account,
                        ThaiTuanERPModules.Expense,
                        ThaiTuanERPModules.Finance,
                        ThaiTuanERPModules.Core,
                };

		private static readonly HashSet<string> AccountEntities = new(StringComparer.OrdinalIgnoreCase)
		{
                        AccountFileEntities.UserAvatar,
		};

		private static readonly HashSet<string> ExpenseEntities = new(StringComparer.OrdinalIgnoreCase)
                {
                        ExpenseFileEntities.Invoice,
                        ExpenseFileEntities.PaymentAttachment,
                        ExpenseFileEntities.OutgoingAttachment,
                        ExpenseFileEntities.CommentAttachment
                };

                public static bool IsValidModule(string module) => Modules.Contains(module);

                public static bool IsValidEntity(string module, string entity)
                {
                        if (module == ThaiTuanERPModules.Expense)
                                return ExpenseEntities.Contains(entity);

                        if(module == ThaiTuanERPModules.Account)
                                return AccountEntities.Contains(entity);

                        return true; // Module khác chưa giới hạn entity
                }
        }
}
