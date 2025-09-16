using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.Divisions.CreateDivision
{
	public class CreateDivisionHandler : IRequestHandler<CreateDivisionCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateDivisionHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(CreateDivisionCommand command, CancellationToken cancellationToken) {
			var division = new Division(command.Name, command.Description, command.HeadUserId);
			await _unitOfWork.Divisions.AddAsync(division);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
