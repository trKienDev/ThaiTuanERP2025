using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Permissions.Queries
{
	public sealed record GetAllPermissionsQuery : IRequest<IReadOnlyList<PermissionDto>>;
	public sealed class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IReadOnlyList<PermissionDto>>
	{
		private readonly IPermissionReadRepository _permissionReadRepo;
		private readonly IMapper _mapper;
		public GetAllPermissionsQueryHandler(IPermissionReadRepository permissionReadRepo, IMapper mapper)
		{
			_mapper = mapper;
			_permissionReadRepo = permissionReadRepo;
		}

		public async Task<IReadOnlyList<PermissionDto>> Handle(GetAllPermissionsQuery query, CancellationToken cancellationToken)
		{
			return await _permissionReadRepo.ListProjectedAsync(
				q => q.Where(p => p.IsActive && !p.IsDeleted)
					.OrderBy(p => p.Code)
					.ProjectTo<PermissionDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}
