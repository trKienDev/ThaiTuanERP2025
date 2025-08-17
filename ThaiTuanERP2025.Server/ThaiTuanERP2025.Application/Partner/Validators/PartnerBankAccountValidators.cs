using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Partner.DTOs;

namespace ThaiTuanERP2025.Application.Partner.Validators
{
	public class PartnerBankAccountValidators : AbstractValidator<UpsertPartnerBankAccountRequest>
	{
		public PartnerBankAccountValidators() {
			RuleFor(x => x.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản không được để trống.")
				.MaximumLength(50);

			RuleFor(x => x.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng không được để trống.")
				.MaximumLength(150);

			RuleFor(x => x.AccountHolder)
				.MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.AccountHolder));

			RuleFor(x => x.SwiftCode)
				.Matches("^[A-Z0-9]{8}([A-Z0-9]{3})?$")
				.When(x => !string.IsNullOrWhiteSpace(x.SwiftCode))
				.WithMessage("SWIFT/BIC phải 8 hoặc 11 ký tự chữ/ số.");

			RuleFor(x => x.Branch)
				.MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.Branch));

			RuleFor(x => x.Note)
				.MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Note));
		}
	}
}
