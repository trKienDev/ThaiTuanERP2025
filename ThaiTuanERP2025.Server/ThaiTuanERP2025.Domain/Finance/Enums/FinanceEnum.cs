using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Finance.Enums
{
	public enum PriceMode { Net = 0, Gross = 1 } // VAT price mode
	public enum WhtBasis { Gross = 0, Net = 1 } // WHT tính theo gross/net
	public enum RoundingRule { None = 0, HalfUp = 1, HalfDown = 2, Bankers = 3, Up = 4, Down = 5 }
}
