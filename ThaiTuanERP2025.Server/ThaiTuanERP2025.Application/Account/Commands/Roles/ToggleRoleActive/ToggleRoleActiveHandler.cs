using MediatR;
using ThaiTuanERP2025.Application.Account.Commands.Roles.ToggleRoleActive;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Roles.UpdateRoleActive
{
	public sealed class ToggleRoleActiveHandler : IRequestHandler<ToggleRoleActiveCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleRoleActiveHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(ToggleRoleActiveCommand commnad, CancellationToken cancellationToken)
		{
			var role = await _unitOfWork.Roles.GetByIdAsync(commnad.Id, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy role yêu cầu");

			if (role.IsActive == true)
				role.Deactivate();
			else if (role.IsActive == false)
				role.Activate();

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
