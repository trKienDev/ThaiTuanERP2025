using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetAllUsers
{
	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			return await _unitOfWork.Users.ListUserDtosWithAvatarAsync(request.Keyword, request.Role, request.DepartmentId, cancellationToken);
		}
	}
}
