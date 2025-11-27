using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.MappingProfiles
{
	public sealed class ExpenseWorkflowTemplateMappingProfile : Profile
	{
		public ExpenseWorkflowTemplateMappingProfile() {
			CreateMap<ExpenseWorkflowTemplate, ExpenseWorkflowTemplateDto>()
				.ForMember(d => d.Steps, opt => opt.MapFrom(src => src.Steps)); 
		}
	}
}
