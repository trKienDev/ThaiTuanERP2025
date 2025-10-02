using System.ComponentModel.DataAnnotations;

namespace ThaiTuanERP2025.Domain.Account.Enums
{
	public enum UserRole
	{
		[Display(Name = "Quản trị viên")]
		admin = 0,
		[Display(Name = "Quản lý")]
		manager = 1,
		[Display(Name = "Nhân viên")]
		user = 2
	}
}
