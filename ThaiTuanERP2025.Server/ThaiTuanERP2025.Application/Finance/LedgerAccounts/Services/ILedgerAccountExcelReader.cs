using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Services
{
        public interface ILedgerAccountExcelReader
        {
                IReadOnlyList<LedgerAccountExcelRow> Read(Stream stream);
        }
}
