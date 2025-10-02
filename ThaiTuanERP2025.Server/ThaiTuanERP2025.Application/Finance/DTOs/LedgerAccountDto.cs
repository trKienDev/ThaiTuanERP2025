﻿using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record LedgerAccountDto(
		Guid Id, 
		string Number, 
		string Name, 
		Guid LedgerAccountTypeId, 
		string LedgerAccountTypeName,
		Guid? ParentLedgerAccountId, 
		LedgerAccountBalanceType LedgerAccountBalanceType,
		string Path, 
		int Level,
		string? Description, 
		bool IsActive
	);

	public sealed record LedgerAccountLookupDto (
		Guid Id, 
		string Number,
		string Name,
		string? Path
	);
}
