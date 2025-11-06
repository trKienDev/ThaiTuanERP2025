using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Departments.Commands
{
	public sealed record SetParentDepartmentCommand(Guid DeptId, Guid ParentId) : IRequest<Unit>;

	public sealed class SetParentDepartmentCommandHandler : IRequestHandler<SetParentDepartmentCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public SetParentDepartmentCommandHandler(IUnitOfWork uow) => _uow = uow;

		public async Task<Unit> Handle(SetParentDepartmentCommand command, CancellationToken cancellationToken)
		{
			var department = await _uow.Departments.GetByIdAsync(command.DeptId, cancellationToken)
				?? throw new KeyNotFoundException("Không tìm thấy phòng ban yêu cầu");

			var parentDepartment = await _uow.Departments.GetByIdAsync(command.ParentId, cancellationToken)
				?? throw new KeyNotFoundException($"Không tìm thấy phòng ban cha");

			department.SetParent(parentDepartment);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
