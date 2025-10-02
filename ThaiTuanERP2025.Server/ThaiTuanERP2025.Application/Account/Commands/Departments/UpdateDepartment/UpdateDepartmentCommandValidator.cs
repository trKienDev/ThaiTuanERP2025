using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment
{
	public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
	{
		public UpdateDepartmentCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên phòng ban không được để trống")
				.MaximumLength(100).WithMessage("Tên phòng ban không được vượt quá 100 ký tự");

		
		}
	}
}
