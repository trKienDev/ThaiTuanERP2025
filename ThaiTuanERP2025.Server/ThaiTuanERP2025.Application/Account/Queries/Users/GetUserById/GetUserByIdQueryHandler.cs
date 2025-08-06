using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetUserById
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
			if (user == null) throw new Exception("User không tồn tại");

			return new UserDto
			{
				Id = user.Id,
				FullName = user.FullName,
				Username = user.Username,
				Email = user.Email?.Value,
				Phone = user.Phone?.Value,
				Role = user.Role,
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
