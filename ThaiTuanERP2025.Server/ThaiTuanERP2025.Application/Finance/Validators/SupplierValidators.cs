using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Validators
{
	public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
	{
		public CreateSupplierRequestValidator()
		{
			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Mã nhà cung cấp không được để trống.")
				.MaximumLength(50).WithMessage("Mã nhà cung cấp không được vượt quá 50 ký tự.")
				.Matches("^[A-Z0-9_\\-\\.]+$").WithMessage("Code chỉ gồm A-Z, 0-9, _-. (viết hoa).");
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhà cung cấp không được để trống.")
				.MaximumLength(200).WithMessage("Tên nhà cung cấp không được vượt quá 200 ký tự.");
			RuleFor(x => x.DefaultCurrency)
				.NotEmpty().WithMessage("Mã tiền tệ mặc định không được để trống.")
				.MaximumLength(3).WithMessage("Mã tiền tệ mặc định phải là 3 ký tự.")
				.Matches("^[A-Z]{3}$").WithMessage("Mã tiền tệ phải là 3 ký tự viết hoa (ví dụ: USD, EUR).");
			RuleFor(x => x.PaymentTermDays)
				.GreaterThanOrEqualTo(0).WithMessage("Số ngày thanh toán phải lớn hơn hoặc bằng 0.")
				.LessThanOrEqualTo(365).WithMessage("Số ngày thanh toán không được vượt quá 365 ngày.")
				.When(x => x.PaymentTermDays.HasValue);
			RuleFor(x => x.WithholdingTaxRate)
				.InclusiveBetween(0, 100).WithMessage("Tỷ lệ thuế khấu trừ phải nằm trong khoảng từ 0% đến 100%.")
				.When(x => x.WithholdingTaxRate.HasValue);
			RuleFor(x => x.Country)
				.MaximumLength(2).WithMessage("Quốc gia không được vượt quá 2 ký tự.")
				.When(x => !string.IsNullOrEmpty(x.Country));
			RuleFor(x => x.Email)
				.EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
			RuleFor(x => x.Phone)
				.MaximumLength(30).WithMessage("Số điện thoại không được vượt quá 30 ký tự.")
				.When(x => !string.IsNullOrWhiteSpace(x.Phone));
		}
	}

	public class UpdateSupplierRequestValidator : AbstractValidator<UpdateSupplierRequest>
	{
		public UpdateSupplierRequestValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhà cung cấp không được để trống.")
				.MaximumLength(200).WithMessage("Tên nhà cung cấp không được vượt quá 200 ký tự.");
			RuleFor(x => x.DefaultCurrency)
				.NotEmpty().WithMessage("Mã tiền tệ mặc định không được để trống.")
				.Length(3).WithMessage("Mã tiền tệ mặc định phải là 3 ký tự.")
				.Must(x => x.All(char.IsLetter));
			RuleFor(x => x.PaymentTermDays)
				.InclusiveBetween(0, 365).WithMessage("Số ngày thanh toán phải nằm trong khoảng từ 0 đến 365.");
			RuleFor(x => x.WithholdingTaxRate)
				.InclusiveBetween(0, 100).WithMessage("Tỷ lệ thuế khấu trừ phải nằm trong khoảng từ 0% đến 100%.")
				.When(x => x.WithholdingTaxRate.HasValue);
			RuleFor(x => x.Country)
				.MaximumLength(2).WithMessage("Quốc gia không được vượt quá 2 ký tự.")
				.When(x => !string.IsNullOrEmpty(x.Country));
			RuleFor(x => x.Email)
				.EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
			RuleFor(x => x.Phone)
				.MaximumLength(30).WithMessage("Số điện thoại không được vượt quá 30 ký tự.")
				.When(x => !string.IsNullOrWhiteSpace(x.Phone));
		}
	}
}
