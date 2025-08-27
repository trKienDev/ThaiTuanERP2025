using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Services
{
	public static class LedgerAccountPathHelper 
	{
		public static void SetPathAndLevel(LedgerAccount entity, LedgerAccount? parent) 
		{ 
			if(parent is null) {
				entity.Path = "/" + entity.Number.Trim();
				entity.Level = 0;
			} else {
				entity.Path = $"{parent.Path.TrimEnd('/')}/{entity.Number.Trim()}";
				entity.Level = parent.Level + 1;
			}
		}
	}
}
