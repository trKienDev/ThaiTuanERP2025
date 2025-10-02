using MediatR;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.CreateCashoutCode
{
	public record CreateCashoutCodeCommand(
		string? Code, string Name, Guid CashoutGroupId, Guid PostingLedgerAccountId, string? Description
	) : IRequest<CashoutCodeDto>;
}
