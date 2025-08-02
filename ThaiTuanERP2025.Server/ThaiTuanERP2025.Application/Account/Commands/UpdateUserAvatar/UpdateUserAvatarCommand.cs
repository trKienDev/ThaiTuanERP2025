using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.UpdateUserAvatar
{
	public class UpdateUserAvatarCommand : IRequest<UserDto>
	{
		public Guid userId { get; }
		public string AvatarUrl { get; }
		public UpdateUserAvatarCommand(Guid userId, string avatarUrl)
		{
			this.userId = userId;
			AvatarUrl = avatarUrl ?? throw new ArgumentNullException(nameof(avatarUrl), "Avatar URL cannot be null");
		}
	}
}
