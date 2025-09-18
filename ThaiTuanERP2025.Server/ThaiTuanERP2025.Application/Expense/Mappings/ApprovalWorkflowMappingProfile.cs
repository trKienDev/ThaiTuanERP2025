using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public class ApprovalWorkflowMappingProfile : Profile
	{
		public ApprovalWorkflowMappingProfile()
		{
			CreateMap<ApprovalStep, ApprovalStepDto>().ReverseMap();
			CreateMap<ApprovalWorkflow, ApprovalWorkflowDto>().ReverseMap();
		}
	}
}
