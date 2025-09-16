using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Divisions.GetAllDivisions
{
	public record GetAllDivisionsQuery : IRequest<List<DivisionSummaryDto>>;
}
