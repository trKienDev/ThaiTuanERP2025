using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.UpdateUserAvatar
{
	public class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand, UserDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		
		public UpdateUserAvatarCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), "Unit of Work cannot be null");
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Mapper cannot be null");
		}

		public async Task<UserDto> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken) {
			var user = await  _unitOfWork.Users.GetByIdAsync(request.userId)
				?? throw new NotFoundException("Người dùng không tồn tại");

			// gọi method domain để cập nhật profile nhưng chỉ đổi avatarUrl
			user.UpdateProfile(
				fullName: user.FullName,
				avatarUrl: request.AvatarUrl,
				position: user.Position,
				email: user.Email,
				phone: user.Phone
			);

			_unitOfWork.Users.Update(user);
			return _mapper.Map<UserDto>(user);
		}
	}
}
