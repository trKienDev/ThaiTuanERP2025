using AutoMapper;
using System.Text.Json;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ApprovalStepInstanceMappingProfile : Profile
	{
		public ApprovalStepInstanceMappingProfile()
		{
			CreateMap<ApprovalStepInstance, ApprovalStepInstanceDto>()
				.ConvertUsing<StepInstanceConverter>();

			CreateMap<ApprovalStepInstance, ApprovalStepInstanceDetailDto>()
				.ConvertUsing<StepInstanceDetailConverter>();

			CreateMap<ApprovalStepInstance, ApprovalStepInstanceStatusDto>();
				//.ConvertUsing<StepInstanceDetailConverter>();
		}

		private sealed class StepInstanceConverter : ITypeConverter<ApprovalStepInstance, ApprovalStepInstanceDto>
		{
			public ApprovalStepInstanceDto Convert(ApprovalStepInstance s, ApprovalStepInstanceDto d, ResolutionContext ctx)
			{
				Guid[]? candidates = null;
				if (!string.IsNullOrWhiteSpace(s.ResolvedApproverCandidatesJson))
				{
					try
					{
						var raw = JsonSerializer.Deserialize<string[]>(s.ResolvedApproverCandidatesJson);
						candidates = raw?.Select(Guid.Parse).ToArray();
					}
					catch { candidates = Array.Empty<Guid>(); }
				}

				object? history = null;
				if (!string.IsNullOrWhiteSpace(s.HistoryJson))
				{
					try { history = JsonSerializer.Deserialize<object>(s.HistoryJson); } catch { }
				}

				return new ApprovalStepInstanceDto(
					s.Id,
					s.WorkflowInstanceId,
					s.TemplateStepId,
					s.Name,
					s.Order,
					s.FlowType == FlowType.OneOfN ? "OneOfN" : "Single",
					s.SlaHours,
					s.ApproverMode == ApproverMode.Condition ? "Condition" : "Standard",
					candidates,
					s.DefaultApproverId,
					s.SelectedApproverId,
					s.Status.ToString(),
					s.StartedAt,
					s.DueAt,
					s.ApprovedAt,
					s.ApprovedBy,
					s.RejectedAt,
					s.RejectedBy,
					s.Comments,
					s.SlaBreached,
					history
				);
			}
		}

		private sealed class StepInstanceDetailConverter : ITypeConverter<ApprovalStepInstance, ApprovalStepInstanceDetailDto>
		{
			public ApprovalStepInstanceDetailDto Convert(ApprovalStepInstance s, ApprovalStepInstanceDetailDto d, ResolutionContext ctx)
			{
				// copy logic parse candidates/history giống StepInstanceConverter
				Guid[]? candidates = null;
				if (!string.IsNullOrWhiteSpace(s.ResolvedApproverCandidatesJson))
				{
					try
					{
						var raw = JsonSerializer.Deserialize<string[]>(s.ResolvedApproverCandidatesJson);
						candidates = raw?.Select(Guid.Parse).ToArray();
					}
					catch { candidates = Array.Empty<Guid>(); }
				}

				object? history = null;
				if (!string.IsNullOrWhiteSpace(s.HistoryJson))
				{
					try { history = JsonSerializer.Deserialize<object>(s.HistoryJson); } catch { }
				}

				return new ApprovalStepInstanceDetailDto(
					s.Id,
					s.WorkflowInstanceId,
					s.TemplateStepId,
					s.Name,
					s.Order,
					s.FlowType == FlowType.OneOfN ? "OneOfN" : "Single",
					s.SlaHours,
					s.ApproverMode == ApproverMode.Condition ? "Condition" : "Standard",
					candidates,
					/* ApproverCandidates */ null, // sẽ fill sau nếu bạn muốn load UserDto
					s.DefaultApproverId,
					s.SelectedApproverId,
					s.Status.ToString(),
					s.StartedAt,
					s.DueAt,
					s.ApprovedAt,
					s.ApprovedBy,
					s.RejectedAt,
					s.RejectedBy,
					s.Comments,
					s.SlaBreached,
					history
				);
			}
		}
	}
}
