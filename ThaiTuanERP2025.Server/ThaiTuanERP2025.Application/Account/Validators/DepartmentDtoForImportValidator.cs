using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Departments.BulkAddDepartmentCommand;

namespace ThaiTuanERP2025.Application.Account.Validators
{
	public class DepartmentDtoForImportValidator : AbstractValidator<DepartmentDtoForImport>
	{
		public DepartmentDtoForImportValidator()
		{
			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã phòng ban không được để trống")
				.MaximumLength(50).WithMessage("Mã phòng ban không được vượt quá 50 ký tự");
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên phòng ban không được để trống")
				.MaximumLength(100).WithMessage("Tên phòng ban không được vượt quá 100 ký tự");
		}
	}
}
