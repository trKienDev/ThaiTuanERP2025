using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Services
{

        public interface ILedgerAccountTypeExcelReader
        {
                IReadOnlyList<LedgerAccountTypeExcelRow> Read(Stream stream);
        }
}
