using ClosedXML.Excel;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Services;

namespace ThaiTuanERP2025.Infrastructure.Finance.Services
{
        public sealed class LedgerAccountTypeExcelReaderService : ILedgerAccountTypeExcelReader
        {
                public IReadOnlyList<LedgerAccountTypeExcelRow> Read(Stream stream)
                {
                        using var workbook = new XLWorkbook(stream);
                        var sheet = workbook.Worksheet(1);

                        var list = new List<LedgerAccountTypeExcelRow>();

                        foreach (var row in sheet.RowsUsed().Skip(1))
                        {
                                list.Add(new LedgerAccountTypeExcelRow
                                {
                                        Code = row.Cell(1).GetString(),
                                        Name = row.Cell(2).GetString(),
                                        Kind = row.Cell(3).GetString(),
                                        Description = row.Cell(4).GetString()
                                });
                        }

                        return list;
                }
        }
}
