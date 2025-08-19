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

			CreateMap<LedgerAccountType, LedgerAccountTypeDto>();

			CreateMap<LedgerAccount, LedgerAccountDto>()
				.ForMember(d => d.AccounTypeName, o => o.MapFrom(s => s.LedgerAccountType.Name));

			CreateMap<Tax, TaxDto>()
				.ForMember(d => d.PostingAccountNumber, o => o.MapFrom(s => s.PostingAccount.Number))
				.ForMember(d => d.PostingAccountNumber, o => o.MapFrom(s => s.PostingAccount.Name));

			CreateMap<CashOutGroup, CashOutGroupDto>();

			CreateMap<CashOutCode, CashOutCodeDto>()
				.ForMember(d => d.CashOutGroupName, o => o.MapFrom(s => s.CashOutGroup.Name))
				.ForMember(d => d.PostingAccountNumber, o => o.MapFrom(s => s.PostingAccount.Number))
				.ForMember(d => d.PostingAccountName, o => o.MapFrom(s => s.PostingAccount.Name));
		}
	}
}
