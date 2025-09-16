using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Divisions.CreateDivision
{
	public record CreateDivisionCommand(string Name, string Description, Guid HeadUserId) : IRequest<Unit>;	
}
