namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts
{
        public sealed record LedgerAccountTypeExcelRow
        {
                public string Code { get; init; } = string.Empty;
                public string Name { get; init; } = string.Empty;
                public string Kind { get; init; } = string.Empty; // raw string
                public string? Description { get; init; }
        }
}
