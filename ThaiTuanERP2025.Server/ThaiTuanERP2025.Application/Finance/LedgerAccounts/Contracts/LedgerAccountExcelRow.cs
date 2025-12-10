namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts
{
        public sealed record LedgerAccountExcelRow
        {
                public string Number { get; init; } = string.Empty;
                public string Name { get; init; } = string.Empty;
                public string BalanceType { get; init; } = "None";
                public string? Description { get; init; }
                public string? LedgerAccountTypeCode { get; init; }
                public string? ParentNumber { get; init; }
        };
}
