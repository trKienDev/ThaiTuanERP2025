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
			Func<T, string?> customerName
		) {
			RuleFor(x => new { Customer = customerName(x) })
				.Must(v => !string.IsNullOrWhiteSpace(v.Customer))
				.WithMessage("Chỉ chọn Department hoặc nhập CustomerName (chỉ một)");
		}
	}
}
