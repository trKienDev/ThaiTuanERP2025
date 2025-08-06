using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Validators;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.BulkAddDepartmentCommand
{
	public class BulkAddDepartmentsCommandValidator : AbstractValidator<BulkAddDepartmentsCommand>
	{
		public BulkAddDepartmentsCommandValidator() {
			RuleFor(x => x.Departments)
				.NotEmpty().WithMessage("Danh sách phòng ban không được để trống.")
				.Must(departments => departments.Count <= 100).WithMessage("Số lượng phòng ban không được vượt quá 100.");
			
			RuleForEach(x => x.Departments)
				.SetValidator(new DepartmentDtoForImportValidator())
				.WithMessage("Thông tin phòng ban không hợp lệ.");
		}
	}
}
