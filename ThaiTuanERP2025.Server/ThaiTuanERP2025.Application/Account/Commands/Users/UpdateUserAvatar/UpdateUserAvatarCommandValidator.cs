using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatar
{
	public class UpdateUserAvatarCommandValidator : AbstractValidator<UpdateUserAvatarCommand>
	{
		public UpdateUserAvatarCommandValidator()
		{
			RuleFor(x => x.userId)
				.NotEmpty().WithMessage("UserId không được để trống.");
			RuleFor(x => x.AvatarUrl)
				.NotEmpty().WithMessage("AvatarUrl không được để trống.")
				.MaximumLength(500).WithMessage("AvatarUrl không được vượt quá 500 ký tự.")
				.Must(BeValidUrl).WithMessage("AvatarUrl không hợp lệ.");
		}

		private bool BeValidUrl(string url)
		{
			return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
		}
	}
}
