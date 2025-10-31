using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ApprovalWorkflowTemplateMappingProfile : Profile
	{
		public ApprovalWorkflowTemplateMappingProfile() {
			CreateMap<ExpenseWorkflowTemplate, ApprovalWorkflowTemplateDto>();
		}	
	}
}
