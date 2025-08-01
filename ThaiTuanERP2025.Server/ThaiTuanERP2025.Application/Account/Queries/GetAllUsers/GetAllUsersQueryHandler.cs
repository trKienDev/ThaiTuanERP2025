using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Application.Account.Queries.GetAllUsers
{
	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
	{
		private readonly IUserRepository _userRepository;

		public GetAllUsersQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			var users = await _userRepository.GetAllAsync();

			// Lọc theo từ khóa
			if (!string.IsNullOrWhiteSpace(request.Keyword))
			{
				users = users.Where(u =>
					u.FullName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
					u.Username.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
 				).ToList();
			}

			// Lọc theo Role
			if (!string.IsNullOrWhiteSpace(request.Role))
			{
				if (Enum.TryParse<UserRole>(request.Role, true, out var parsedRole))
				{
					users = users.Where(u => u.Role == parsedRole).ToList();
				}
			}

			// Lọc theo DepartmentId
			if (request.DepartmentId.HasValue)
			{
				users = users.Where(u => u.DepartmentId == request.DepartmentId).ToList();
			}

			return users.Select(u => new UserDto
			{
				Id = u.Id,
				FullName = u.FullName,
				Username = u.Username,
				Position = u.Position,
				Email = u.Email?.Value,
				Phone = u.Phone?.Value,
				Role = u.Role,
				DepartmentId = u.DepartmentId,
			}).ToList();
		}
	}
}
