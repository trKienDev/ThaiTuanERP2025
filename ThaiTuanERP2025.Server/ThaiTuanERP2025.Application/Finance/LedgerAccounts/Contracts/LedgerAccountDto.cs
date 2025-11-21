using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts
{
        public sealed record LedgerAccountDto
        {
                public Guid Id { get; init; }
                public string Number { get; init; } = null!;
                public string Name { get; init; } = null!;

                public Guid? LedgerAccountTypeId { get; init; }
                public LedgerAccountTypeDto? LedgerAccountType { get; init; }

                public Guid? ParentLedgerAccountId { get; init; }
                public string? Description { get; init; }
                public int Level { get; init; }
                public string Path { get; init; } = null!;
                public LedgerAccountBalanceType BalanceType { get; init; }
                public bool IsActive { get; init; }
        }

        public sealed record LedgerAccountTreeDto
        {
		public Guid Id { get; init; }
		public Guid? ParentId { get; init; }

		public string Number { get; init; } = default!;
		public string Name { get; init; } = default!;
		public LedgerAccountBalanceType BalanceType { get; init; }

		public int Level { get; init; }
		public string Path { get; init; } = default!;

		public string? LedgerAccountTypeName { get; init; }
		public string? Description { get; init; }

	}
}
