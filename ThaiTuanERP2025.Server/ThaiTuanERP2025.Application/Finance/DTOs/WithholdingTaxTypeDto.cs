using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record WithholdingTaxTypeDto(
		Guid Id,
		string Name,
		decimal Rate,
		string Description,
		bool IsActive
	);
}
