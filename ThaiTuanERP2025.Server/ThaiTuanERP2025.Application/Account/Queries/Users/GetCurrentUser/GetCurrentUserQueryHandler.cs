using AutoMapper;
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

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetCurrentUser
{
	public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
		{
			var userIdClaim = request.UserPrincipal.FindFirst(ClaimTypes.NameIdentifier)
				?? throw new AppException("Không tìm thấy ID người dùng");
			if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
				throw new AppException("Id người dùng không hợp lệ");
			var user = await _unitOfWork.Users.GetByIdAsync(userId)
				?? throw new NotFoundException("Người dùng không tồn tại");

			return _mapper.Map<UserDto>(user);
		}
	}
}
