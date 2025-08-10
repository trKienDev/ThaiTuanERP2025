using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Validators
{
	public class BankAccountOwnerValidator<T> : AbstractValidator<T>
	{
		public BankAccountOwnerValidator(
			Func<T, Guid?> departmentId,
			Func<T, string?> customerName
		) {
			RuleFor(x => new { Department = departmentId(x), Customer = customerName(x) })
				.Must(v => (v.Department.HasValue ^ !string.IsNullOrWhiteSpace(v.Customer)))
				.WithMessage("Chỉ chọn Department hoặc nhập CustomerName (chỉ một)");
		}
	}
}
