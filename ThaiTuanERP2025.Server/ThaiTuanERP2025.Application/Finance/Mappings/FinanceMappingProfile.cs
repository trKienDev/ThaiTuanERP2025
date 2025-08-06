using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Mappings
{
	public class FinanceMappingProfile : Profile
	{
		public FinanceMappingProfile() { 
			CreateMap<BudgetGroup, BudgetGroupDto>();
			CreateMap<BudgetCode, BudgetCodeDto>();
		}
	}
}
