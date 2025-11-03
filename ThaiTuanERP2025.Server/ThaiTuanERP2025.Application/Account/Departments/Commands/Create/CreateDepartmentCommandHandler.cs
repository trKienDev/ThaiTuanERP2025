using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Departments.Commands.Create
{
	public sealed class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateDepartmentCommandHandler(IUnitOfWork uow) {
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken) {
			Department? parentDept = null;
			if (command.ParentId.HasValue)
				parentDept = await _uow.Departments.GetByIdAsync(command.ParentId.Value)
					?? throw new NotFoundException("Không tìm thấy phòng ban cha");

			var entity = new Department(command.Name, command.Code, command.ManagerId);
			if (parentDept is not null)
				entity.SetParent(parentDept);

			await _uow.Departments.AddAsync(entity);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	} 
}
