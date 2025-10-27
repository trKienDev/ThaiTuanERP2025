using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Queries.Permissions.GetPermissionsByRoleId
{
	public class GetPermissionsByRoleIdHandler : IRequestHandler<GetPermissionsByRoleIdQuery, IEnumerable<PermissionDto>> 
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetPermissionsByRoleIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByRoleIdQuery query, CancellationToken cancellationToken)
		{
			var role = await _unitOfWork.Roles.GetByIdAsync(query.RoleId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy role yêu cầu");

			var permissions = await _unitOfWork.Permissions.FindAsync(
				p => p.RolePermissions.Any(rp => rp.RoleId == query.RoleId),
				cancellationToken: cancellationToken);

			return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
		}
	}
}
