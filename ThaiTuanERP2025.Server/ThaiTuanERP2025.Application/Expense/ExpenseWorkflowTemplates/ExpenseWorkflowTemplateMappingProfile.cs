using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates
{
	public sealed class ExpenseWorkflowTemplateMappingProfile : Profile
	{
		public ExpenseWorkflowTemplateMappingProfile() {
			CreateMap<ExpenseWorkflowTemplate, ExpenseWorkflowTemplateDto>();
		}
	}
}
