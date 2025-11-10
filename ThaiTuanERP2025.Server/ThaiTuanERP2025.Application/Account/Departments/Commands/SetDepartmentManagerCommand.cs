using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Departments.Commands
{
	public sealed record SetDepartmentManagersRequest(Guid PrimaryManagerId, IReadOnlyList<Guid> ViceManagerIds );
	public sealed record SetDepartmentManagerCommand(Guid DepartmentId, SetDepartmentManagersRequest Request) : IRequest<Unit>;

	public sealed class SetDepartmentManagerCommandHandler : IRequestHandler<SetDepartmentManagerCommand, Unit> {
		private readonly IUnitOfWork _uow;
		public SetDepartmentManagerCommandHandler(IUnitOfWork uow) => _uow = uow;
		
		public async Task<Unit> Handle(SetDepartmentManagerCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			Guard.AgainstNullOrEmptyGuid(request.PrimaryManagerId, nameof(request.PrimaryManagerId));
			foreach(var viceId in request.ViceManagerIds) 
				Guard.AgainstNullOrEmptyGuid(viceId, nameof(viceId));
			Guard.AgainstNullOrEmptyGuid(command.DepartmentId, nameof(command.DepartmentId));

			var primaryManager = await _uow.Users.GetByIdAsync(request.PrimaryManagerId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy user - quản lý chính");

			var viceIds = request.ViceManagerIds.Distinct().Where(id => id != request.PrimaryManagerId).ToList();
			var viceManagers = await _uow.Users.FindAsync(u => viceIds.Contains(u.Id), cancellationToken);

			var foundIds = viceManagers.Select(u => u.Id).ToHashSet();
			var missing = viceIds.Where(id => !foundIds.Contains(id)).ToList();
			if (missing.Count > 0)
				throw new NotFoundException($"Không tìm thấy user (phó quản lý): {string.Join(", ", missing)}");

			var department = await _uow.Departments.SingleOrDefaultIncludingAsync(
				d => d.Id == command.DepartmentId,
				asNoTracking: false,
				cancellationToken: cancellationToken,
				d => d.Managers
			) ?? throw new NotFoundException("Không tìm thấy phòng ban yêu cầu");

			//if (primaryManager.DepartmentId != department.Id)
			//	throw new DomainException("Quản lý chính phải thuộc phòng ban này.");

			var invalidVice = viceManagers.Where(u => u.DepartmentId != department.Id).Select(u => u.Id).ToList();
			if (invalidVice.Any())
				throw new DomainException($"Các user không thuộc phòng ban: {string.Join(", ", invalidVice)}");

			foreach (var vm in viceManagers)
				department.AssignManager(vm);

			department.AssignManager(primaryManager);
			department.SetPrimaryManager(primaryManager.Id);

			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
 }
