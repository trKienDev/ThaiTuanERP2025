using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.MappingProfiles
{
	public sealed class ExpenseWorkflowInstanceMappingProfile : Profile
	{
		public ExpenseWorkflowInstanceMappingProfile()
		{
			CreateMap<ExpenseWorkflowInstance, ExpenseWorkflowInstanceDto>();

			CreateMap<ExpenseWorkflowInstance, ExpenseWorkflowInstanceBriefDto>()
				.ForMember(
					dest => dest.Steps,
					opt => opt.MapFrom(src =>
						src.Steps.Where(s => s.Status == ExpenseStepStatus.Approved || s.Status == ExpenseStepStatus.Waiting).OrderBy(s => s.Order)
					)
				);
		}
	}
}
