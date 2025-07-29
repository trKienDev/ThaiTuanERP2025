using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.GetAllUsers
{
	public class GetAllUsersQuery : IRequest<List<UserDto>>
	{
		public string? Keyword { get; set; }
		public string? Role { get; set; }
		public Guid? DepartmentId { get; set; }

		public GetAllUsersQuery(string? keyword, string? role, Guid? departmentId)
		{
			Keyword = keyword;
			Role = role;
			DepartmentId = departmentId;
		}
	}
}
