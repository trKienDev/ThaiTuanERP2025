using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates
{
	public sealed class ExpenseStepTemplateMappingProfile : Profile
	{
		public ExpenseStepTemplateMappingProfile()
		{
			CreateMap<ExpenseStepTemplate, ExpenseStepTemplateDto>();
		}
	}
}
