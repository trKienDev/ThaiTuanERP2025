using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public class RemoveUserDto
	{
		public Guid UserId { get; set; }
		public Guid RequestorId { get; set; } // Id của người gửi yêu cầu xóa
	}
}
