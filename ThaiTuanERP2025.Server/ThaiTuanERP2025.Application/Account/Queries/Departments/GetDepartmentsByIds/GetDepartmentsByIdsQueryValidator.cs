using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetDepartmentsByIds
{
	public class GetDepartmentsByIdsQueryValidator : AbstractValidator<GetDepartmentsByIdsQuery>
	{
		public GetDepartmentsByIdsQueryValidator()
		{
			RuleFor(x => x.DepartmentIds)
				.NotEmpty().WithMessage("DepartmentIds không được để trống.")
				.NotNull().WithMessage("DepartmentIds không được null.");

			RuleForEach(x => x.DepartmentIds)
				.NotEmpty().WithMessage("Mỗi DepartmentId không được để trống.")
				.Must(id => id != Guid.Empty).WithMessage("Id phòng ban không hợp lệ");	
		}
	}
}
