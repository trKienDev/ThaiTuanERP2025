using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.CreateTax
{
	public class CreateTaxValidator : AbstractValidator<CreateTaxCommand>
	{
		public CreateTaxValidator(IUnitOfWork unitOfWork) {
			RuleFor(x => x.PolicyName)
				.NotEmpty().WithMessage("Tên chính sách thuế không được để trống")
				.MaximumLength(200).WithMessage("Tên chính sách thuế không vượt quá 200 ký tự")
				.MustAsync(async(name, ct) => 
					!await unitOfWork.Taxes.PolicyNameExistsAsync(name.Trim(), null, ct)
				).WithMessage("Chính sách thuế đã tồn tại");

			RuleFor(x => x.Rate).InclusiveBetween(0, 100).WithMessage("% phải từ 0 - 100%");
			RuleFor(x => x.PostingLedgerAccountId)
				.NotEmpty().WithMessage("Tài khoản hạch toán không được để trống")
				.MustAsync((id, ct) => unitOfWork.LedgerAccounts.AnyAsync(a => a.Id == id)).WithMessage("Tài khoản hạch toán không tồn tại");
		}
	}
}
