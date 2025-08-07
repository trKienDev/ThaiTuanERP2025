using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment
{
	public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
	{
		public DeleteDepartmentCommandValidator()
		{
			RuleFor(x => x.id)
				.NotEmpty().WithMessage("api không lấy được id, vui lòng kiểm tra lại");
		
		}
	}
}
