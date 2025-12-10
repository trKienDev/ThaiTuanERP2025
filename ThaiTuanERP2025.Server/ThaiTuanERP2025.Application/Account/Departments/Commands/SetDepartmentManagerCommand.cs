using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Services;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Account.Departments.Commands
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="PrimaryManagerId"></param>
	/// <param name="ViceManagerIds"></param>
	/// <param name="CascadeToMembers"></param>
	/// <param name="ReplaceMode"></param>
	public sealed record SetDepartmentManagersRequest(
		Guid PrimaryManagerId, IReadOnlyList<Guid> ViceManagerIds, 
		bool CascadeToMembers,
		bool ReplaceMode  // true = replace, false = merge
	);
	public sealed record SetDepartmentManagerCommand(Guid DepartmentId, SetDepartmentManagersRequest Request) : IRequest<Unit>;

	public sealed class SetDepartmentManagerCommandHandler : IRequestHandler<SetDepartmentManagerCommand, Unit> {
		private readonly IUnitOfWork _uow;
		private readonly IUserManagerService _managerService;
		public SetDepartmentManagerCommandHandler(IUnitOfWork uow, IUserManagerService managerService)
		{
			_uow = uow;
			_managerService = managerService;
		}
		
		public async Task<Unit> Handle(SetDepartmentManagerCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			foreach (var viceId in request.ViceManagerIds)
				Guard.AgainstNullOrEmptyGuid(viceId, nameof(viceId));

			// 1 ) Load department
			var department = await _uow.Departments.SingleOrDefaultIncludingAsync(
				d => d.Id == command.DepartmentId,
				asNoTracking: false,
				cancellationToken: cancellationToken,
				d => d.Managers,
				d => d.Users
			) ?? throw new NotFoundException("Không tìm thấy phòng ban yêu cầu");

			// 2) Load primary
			var primaryManager = await _uow.Users.GetByIdAsync(request.PrimaryManagerId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy user - quản lý trưởng");

			// 3 ) Load vice managers
			var viceIds = request.ViceManagerIds
				.Distinct()
				.Where(id => id != request.PrimaryManagerId)
				.ToList();

			var viceManagers = await _uow.Users.FindAsync(u => viceIds.Contains(u.Id), cancellationToken);
			var foundViceIds = viceManagers.Select(u => u.Id).ToHashSet();
			var missing = viceIds.Where(id => !foundViceIds.Contains(id)).ToList();
			if (missing.Any())
				throw new NotFoundException($"Không tìm thấy user (phó quản lý): {string.Join(", ", missing)}");

			var invalidVice = viceManagers.Where(u => u.DepartmentId != department.Id).Select(u => u.Id).ToList();
			if (invalidVice.Any())
				throw new DomainException($"Các user không thuộc phòng ban: {string.Join(", ", invalidVice)}");

			// 4 )  Assign managers to department
			foreach (var vm in viceManagers)
				department.AssignManager(vm);

			department.AssignManager(primaryManager);
			department.SetPrimaryManager(primaryManager.Id);

			// 5) Cascade to members
			if (request.CascadeToMembers)
			{
				var deptManagerIds = new List<Guid> { primaryManager.Id };
				deptManagerIds.AddRange(viceManagers.Select(v => v.Id));

				foreach (var user in department.Users)
				{
					if (user.Id == primaryManager.Id) continue;
					if (viceIds.Contains(user.Id)) continue;

					if (request.ReplaceMode)
						await _managerService.ReplaceAsync(user.Id, deptManagerIds, primaryManager.Id, cancellationToken);
					else
						await _managerService.MergeAsync(user.Id, deptManagerIds, primaryManager.Id, cancellationToken);
				}
			}


			await _uow.SaveChangesAsync(cancellationToken);


			return Unit.Value;
		}
	}

	public sealed class SetDepartmentManagerCommandValidator : AbstractValidator<SetDepartmentManagerCommand>
	{
		public SetDepartmentManagerCommandValidator() {
			RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Id phòng ban không được đẻ trống");
			RuleFor(x => x.Request.PrimaryManagerId).NotEmpty().WithMessage("Id quản lý chính không được để trống");
		}
	}
 }
