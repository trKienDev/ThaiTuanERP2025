using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.MappingProfiles
{
	public sealed class ExpenseWorkflowInstanceMappingProfile : Profile
	{
		public ExpenseWorkflowInstanceMappingProfile()
		{
			CreateMap<ExpenseWorkflowInstance, ExpenseWorkflowInstanceDto>();
		}
	}
}
