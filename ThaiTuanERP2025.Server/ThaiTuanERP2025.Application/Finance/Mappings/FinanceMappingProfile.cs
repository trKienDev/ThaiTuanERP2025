using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Dtos;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Partner.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Partner.Entities;

namespace ThaiTuanERP2025.Application.Finance.Mappings
{
	public class FinanceMappingProfile : Profile
	{
		public FinanceMappingProfile() { 
			CreateMap<BudgetGroup, BudgetGroupDto>();

			CreateMap<BudgetCode, BudgetCodeDto>();

			CreateMap<BudgetPeriod, BudgetPeriodDto>();

			CreateMap<BudgetPlan, BudgetPlanDto>()
				.ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
				.ForMember(dest => dest.BudgetCodeName, opt => opt.MapFrom(src => src.BudgetCode.Name))
				.ForMember(dest => dest.BudgetPeriodName, opt => opt.MapFrom(src => $"{src.BudgetPeriod.Month:D2}/{src.BudgetPeriod.Year}"));

			CreateMap<BankAccount, BankAccountDto>();

			
		}
	}
}
