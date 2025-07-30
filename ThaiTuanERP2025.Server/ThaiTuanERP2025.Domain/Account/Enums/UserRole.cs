using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Account.Enums
{
	public enum UserRole
	{
		[Display(Name = "Quản trị viên")]
		admin = 0,
		[Display(Name = "Quản lý")]
		manager = 1,
		[Display(Name = "Nhân viên")]
		employee = 2
	}
}
