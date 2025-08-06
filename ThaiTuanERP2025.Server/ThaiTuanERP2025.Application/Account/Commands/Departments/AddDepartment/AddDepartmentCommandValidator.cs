using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment
{
	public class AddDepartmentCommandValidator : AbstractValidator<AddDepartmentCommand>
	{
		public AddDepartmentCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên phòng ban không được để trống")
				.MaximumLength(100).WithMessage("Tên phòng ban không được vượt quá 100 ký tự");

			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã phòng ban không được để trống")
				.MaximumLength(20).WithMessage("Mã phòng ban không được vượt quá 50 ký tự");
		}
	}
}
