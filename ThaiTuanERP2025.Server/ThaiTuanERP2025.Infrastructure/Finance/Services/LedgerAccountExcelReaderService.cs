using ClosedXML.Excel;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Services;

namespace ThaiTuanERP2025.Infrastructure.Finance.Services
{
        public sealed class LedgerAccountExcelReaderService : ILedgerAccountExcelReader
        {
                public IReadOnlyList<LedgerAccountExcelRow> Read(Stream stream)
                {
                        using var workbook = new XLWorkbook(stream);
                        var sheet = workbook.Worksheet(1);

                        var list = new List<LedgerAccountExcelRow>();

                        foreach (var row in sheet.RowsUsed().Skip(1)) // bỏ dòng title
                        {
                                list.Add(new LedgerAccountExcelRow
                                {
                                        Number = row.Cell(1).GetString(),
                                        Name = row.Cell(2).GetString(),
                                        BalanceType = row.Cell(3).GetString(),
                                        Description = row.Cell(4).GetString(),
                                        LedgerAccountTypeCode = row.Cell(5).GetString(),
                                        ParentNumber = row.Cell(6).GetString()
                                });
                        }

                        return list;
                }
        }
}
