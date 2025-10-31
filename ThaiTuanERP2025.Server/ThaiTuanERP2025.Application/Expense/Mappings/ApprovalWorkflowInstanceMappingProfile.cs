using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ApprovalWorkflowInstanceMappingProfile : Profile
	{
		public ApprovalWorkflowInstanceMappingProfile() {
			CreateMap<ExpenseWorkflowInstance, ApprovalWorkflowInstanceDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.Steps, opt => opt.MapFrom(s => s.Steps != null
					? s.Steps.OrderBy(x => x.Order).ToList()
					: new List<ExpenseStepInstance>()));

			CreateMap<ExpenseWorkflowInstance, ApprovalWorkflowInstanceDetailDto>()
				.ForMember(d => d.WorkflowInstance, opt => opt.MapFrom(s => s))
				.ForMember(d => d.Steps, opt => opt.MapFrom(s => s.Steps ?? new List<ExpenseStepInstance>()));

			CreateMap<ExpenseWorkflowInstance, ApprovalWorkflowInstanceStatusDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.Steps, opt => opt.MapFrom(
					s => s.Steps != null
						? s.Steps.OrderBy(x => x.Order).ToList()
						: new List<ExpenseStepInstance>()
				));
		}
	}
}
