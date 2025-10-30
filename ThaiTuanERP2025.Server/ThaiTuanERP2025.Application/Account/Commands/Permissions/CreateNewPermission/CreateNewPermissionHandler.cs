using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Permissions.CreateNewPermission
{
	public sealed class CreateNewPermissionHandler : IRequestHandler<CreateNewPermissionCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateNewPermissionHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Unit> Handle(CreateNewPermissionCommand command, CancellationToken cancellationToken) {
			var request = command.request;

			var name = request.Name.Trim();
			var code = request.Code.Trim();
			var description = request.Description.Trim();

			var existed = await _unitOfWork.Permissions.SingleOrDefaultAsync(
				q => q.Where(p => p.Name == name || p.Code == code),
				cancellationToken: cancellationToken
			);
			if(existed is not null) throw new NotFoundException("Quyền này đã tồn tại");

			var newPermission = new Permission(name, code, description);
			await _unitOfWork.Permissions.AddAsync(newPermission, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
