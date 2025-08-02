using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Queries.GetCurrentUser
{
	public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto> 
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken) {
			var userIdClaim = request.UserPrincipal.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new AppException("Không tìm thấy ID người dùng");
			if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
				throw new AppException("Id người dùng không hợp lệ");
			var user = await _unitOfWork.Users.GetByIdAsync(userId)
				?? throw new NotFoundException("Người dùng không tồn tại");

			return new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				FullName = user.FullName,
				EmployeeCode = user.EmployeeCode,
				Email = user.Email?.Value,
				Phone = user.Phone?.Value,
				Position = user.Position,
				Role = user.Role,
				AvatarUrl = user.AvatarUrl,
				DepartmentId = user.DepartmentId,
				Department = user.Department is null ? null : new DepartmentDto
				{
					Id = user.Department.Id,
					Name = user.Department.Name,
					Code = user.Department.Code
				}
			};
		}
	}
}
