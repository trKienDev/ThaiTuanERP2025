using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Suppliers.UpdateSupplier
{
	public sealed class UpdateSupplierValidator : AbstractValidator<UpdateSupplierRequest>
	{
		public UpdateSupplierValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhà cung cấp không được đẻ trống")
				.MaximumLength(256).WithMessage("Tên không được vượt quá 256 ký tự");
			RuleFor(x => x.TaxCode)
				.MaximumLength(32).WithMessage("Mã nhà cung cấp không vượt quá 32 ký tự");
		}
	}
}
