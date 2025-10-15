using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ApprovalWorkflowInstanceMappingProfile : Profile
	{
		public ApprovalWorkflowInstanceMappingProfile() {
			CreateMap<ApprovalWorkflowInstance, ApprovalWorkflowInstanceDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.Steps, opt => opt.MapFrom(s => s.Steps.OrderBy(x => x.Order).ToList()));

			CreateMap<ApprovalWorkflowInstance, ApprovalWorkflowInstanceDetailDto>()
			    .ForMember(d => d.WorkflowInstance, opt => opt.MapFrom(s => s))
			    .ForMember(d => d.Steps, opt => opt.MapFrom(s => s.Steps));

			CreateMap<ApprovalWorkflowInstance, ApprovalWorkflowInstanceStatusDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.Steps, opt => opt.MapFrom(s => s.Steps));
		}
	}
}
