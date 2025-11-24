using AutoMapper;
using System.Text.Json;
using ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates
{
	public sealed class ExpenseStepTemplateMappingProfile : Profile
	{
		public ExpenseStepTemplateMappingProfile()
		{
			CreateMap<ExpenseStepTemplate, ExpenseStepTemplateDto>()
                                .ForMember(d => d.ApproverIds, opt => opt.Ignore())
    .AfterMap((src, dest) =>
    {
            dest.ApproverIds =
                string.IsNullOrEmpty(src.FixedApproverIdsJson)
                ? new()
                : JsonSerializer.Deserialize<List<Guid>>(src.FixedApproverIdsJson)
                  ?? new();
    });
                }
	}
}
