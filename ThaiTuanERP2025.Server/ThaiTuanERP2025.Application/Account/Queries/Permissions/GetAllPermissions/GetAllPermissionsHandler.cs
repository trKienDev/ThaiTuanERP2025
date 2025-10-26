using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Queries.Permissions.GetAllPermissions
{
	public class GetAllPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllPermissionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery query, CancellationToken cancellationToken) {
			var permissions = await _unitOfWork.Permissions.GetAllAsync(cancellationToken);
			return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
		}
	}
}
