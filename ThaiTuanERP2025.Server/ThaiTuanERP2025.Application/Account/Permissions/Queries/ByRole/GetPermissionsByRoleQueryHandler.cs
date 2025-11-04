using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Roles;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Permissions.Queries.ByRole
{
	public class GetPermissionsByRoleIdHandler : IRequestHandler<GetPermissionsByRoleIdQuery, IReadOnlyList<PermissionDto>>
	{
		private readonly IPermissionReadRepository _permissionReadRepo;
		private readonly IRoleReadRepository _roleReadRepo;
		private readonly IMapper _mapper;
		public GetPermissionsByRoleIdHandler(
			IPermissionReadRepository permissionReadRepo, IMapper mapper, IRoleReadRepository roleReadRepo
		)
		{
			_permissionReadRepo = permissionReadRepo;
			_mapper = mapper;
			_roleReadRepo = roleReadRepo;
		}

		public async Task<IReadOnlyList<PermissionDto>> Handle(GetPermissionsByRoleIdQuery query, CancellationToken cancellationToken)
		{
			var roleExist = await _roleReadRepo.ExistAsync(q => q.Id == query.RoleId, cancellationToken);
			if (roleExist is false) throw new ConflictException("Role không tồn tại");

			return await _permissionReadRepo.ListProjectedAsync(
				q => q.Where(p => p.RolePermissions.Any(rp => rp.RoleId == query.RoleId))
				.ProjectTo<PermissionDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}
