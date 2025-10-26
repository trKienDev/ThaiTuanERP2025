using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Queries.Roles.GetAllRoles
{
	public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllRolesHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
		{
			var roles = await _unitOfWork.Roles.GetAllIncludingAsync(
				cancellationToken,
				r => r.RolePermissions,
				r => r.RolePermissions.Select(rp => rp.Permission)
			);

			return _mapper.Map<IEnumerable<RoleDto>>(roles);
		}
	}
}
