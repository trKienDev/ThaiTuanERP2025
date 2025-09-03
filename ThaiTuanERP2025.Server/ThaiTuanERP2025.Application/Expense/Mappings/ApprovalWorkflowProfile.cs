using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public class ApprovalWorkflowProfile : Profile
	{
		public ApprovalWorkflowProfile()
		{
			// Map Workflow --> WorkflowDto 
			CreateMap<ApprovalWorkflow, ApprovalWorkflowDto>()
				.ForMember(d => d.Steps, opt => opt.MapFrom(s => s.Steps.OrderBy(x => x.Order)));

			// Map Step -> StepDto, parse CandidateJson trong AfterMap
			CreateMap<ApprovalStep, ApprovalStepDto>()
				.ForMember(d => d.CandidateUserIds, opt => opt.Ignore())
				.AfterMap((src, dest) =>
				{
					try
					{
						var list = JsonSerializer.Deserialize<List<Guid>>(src.CandidateJson, (JsonSerializerOptions?)null) ?? new List<Guid>();
						dest.CandidateUserIds = list.Select(g => g.ToString()).ToArray();
					}
					catch
					{
						// Nếu JSON xấu, fallback = []
						dest.CandidateUserIds = Array.Empty<string>();
					}
				});
		}
	}
}
