using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Account.Roles.Queries.GetAllRoles
{
	public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
	{
		private readonly IRoleReadRepository _roleReadRepo;
		private readonly IMapper _mapper;
		public GetAllRolesQueryHandler(IRoleReadRepository roleReadRepo, IMapper mapper)
		{
			_roleReadRepo = roleReadRepo;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
		{
			return await _roleReadRepo.ListProjectedAsync(
				q => q.Where(r => r.IsActive)
					.ProjectTo<RoleDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}
