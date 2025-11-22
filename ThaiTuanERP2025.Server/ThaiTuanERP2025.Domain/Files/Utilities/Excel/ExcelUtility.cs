using System.Text;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Files.Utilities
{
	public static class ExcelHelper
	{
		public static List<LedgerAccountTypePayload> ReadLedgerAccountTypeFile(IFormFile file)
		{
			using var stream = file.OpenReadStream();
			using var workbook = new XLWorkbook(stream);
			var sheet = workbook.Worksheet(1);

			var list = new List<LedgerAccountTypePayload>();

			foreach (var row in sheet.RowsUsed().Skip(1)) // bỏ header
			{
				list.Add(new LedgerAccountTypePayload
				{
					Code = row.Cell(1).GetString(),
					Name = row.Cell(2).GetString(),
					Kind = Enum.Parse<LedgerAccountTypeKind>(row.Cell(3).GetString()),
					Description = row.Cell(4).GetString()
				});
			}

			return list;
		}
	}
}
