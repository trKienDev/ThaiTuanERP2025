using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.CreateRole
{
	public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateRoleHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
		{
			var exists = await _unitOfWork.Roles.SingleOrDefaultIncludingAsync(r => r.Name == request.Name, cancellationToken: cancellationToken);
			if (exists is not null)
				throw new InvalidOperationException($"Role '{request.Name}' đã tồn tại.");

			var role = new Role(request.Name, request.Description);

			await _unitOfWork.Roles.AddAsync(role, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return role.Id;
		}
	}
}
