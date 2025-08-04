using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Validators
{
	public class ChangeAdminDtoValidator : AbstractValidator<ChangeAdminDto>
	{
		public ChangeAdminDtoValidator()
		{
			RuleFor(x => x.NewAdminId).NotEmpty();
			RuleFor(x => x.RequestorId).NotEmpty();
		}
	}
}
