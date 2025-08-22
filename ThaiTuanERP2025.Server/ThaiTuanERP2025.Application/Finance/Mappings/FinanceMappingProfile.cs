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
				.ForCtorParam(nameof(LedgerAccountDto.LedgerAccountTypeName), opt => opt.MapFrom(src => src.LedgerAccountType.Name))
				.ForCtorParam(nameof(LedgerAccountDto.ParentLedgerAccountId),opt => opt.MapFrom(src => src.ParentLedgerAccountId));

			CreateMap<Tax, TaxDto>()
				.ForMember(d => d.PostingLedgerAccountNumber, o => o.MapFrom(s => s.PostingLedgerAccount.Number))
				.ForMember(d => d.PostingLedgerAccountNumber, o => o.MapFrom(s => s.PostingLedgerAccount.Name));

			CreateMap<CashOutGroup, CashOutGroupDto>();

			CreateMap<CashOutCode, CashOutCodeDto>()
				.ForMember(d => d.CashOutGroupCode, o => o.MapFrom(s => s.CashOutGroup.Code))
				.ForMember(d => d.CashOutGroupName, o => o.MapFrom(s => s.CashOutGroup.Name))
				.ForMember(d => d.PostingLedgerAccountId, o => o.MapFrom(s => s.PostingLedegerAccoutnId))
				.ForMember(d => d.PostingLedgerAccountNumber, o => o.MapFrom(s => s.PostingLedgerAccount.Number))
				.ForMember(d => d.PostingLedgerAccountName, o => o.MapFrom(s => s.PostingLedgerAccount.Name));
		}
	}
}
