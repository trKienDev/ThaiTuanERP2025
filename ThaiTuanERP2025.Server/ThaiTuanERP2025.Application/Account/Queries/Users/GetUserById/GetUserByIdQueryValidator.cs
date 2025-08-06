using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetUserById
{
	public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
	{
		public GetUserByIdQueryValidator() { 
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id không được để trống.")
				.NotNull().WithMessage("Id không được null.")
				.Must(id => id != Guid.Empty).WithMessage("Id người dùng không hợp lệ");
		}
	}
}
