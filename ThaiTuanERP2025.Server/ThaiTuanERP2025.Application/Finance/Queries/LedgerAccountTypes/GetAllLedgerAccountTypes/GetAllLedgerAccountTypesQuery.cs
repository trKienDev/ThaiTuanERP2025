using MediatR;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccountTypes.GetAllLedgerAccountTypes
{
	public record GetAllLedgerAccountTypesQuery() : IRequest<List<LedgerAccountTypeDto>>;
}
