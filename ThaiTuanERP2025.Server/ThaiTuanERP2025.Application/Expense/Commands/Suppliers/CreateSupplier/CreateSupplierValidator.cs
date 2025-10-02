using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Suppliers.CreateSupplier
{
	public sealed class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
	{
		public CreateSupplierValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhà cung cấp không được để trống")
				.MaximumLength(256).WithMessage("Độ dài tối đa 256 ký tự");
			RuleFor(x => x.TaxCode)
				.MaximumLength(32).WithMessage("Mã nhà cung cấp tối đa 32 ký tự");
		}
	}
}
