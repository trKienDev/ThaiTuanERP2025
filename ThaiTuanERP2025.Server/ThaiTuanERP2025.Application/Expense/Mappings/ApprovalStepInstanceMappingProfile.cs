using AutoMapper;
using System.Text.Json;
using ThaiTuanERP2025.Application.Account.Dtos;
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

			CreateMap<ApprovalStepInstance, ApprovalStepInstanceStatusDto>()
				.ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.ApprovedByUser, o => o.MapFrom<ApprovedUserResolver>())
				.ForMember(d => d.RejectedByUser, o => o.MapFrom<RejectedUserResolver>());
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

		private sealed class ApprovedUserResolver : IValueResolver<ApprovalStepInstance, ApprovalStepInstanceStatusDto, UserDto?>
		{
			public UserDto? Resolve(ApprovalStepInstance s, ApprovalStepInstanceStatusDto d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.ApprovedBy.HasValue) return null;
				return TryGetUserDto(ctx, s.ApprovedBy.Value);
			}
		}

		private sealed class RejectedUserResolver : IValueResolver<ApprovalStepInstance, ApprovalStepInstanceStatusDto, UserDto?>
		{
			public UserDto? Resolve(ApprovalStepInstance s, ApprovalStepInstanceStatusDto d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.RejectedBy.HasValue) return null;
				return TryGetUserDto(ctx, s.RejectedBy.Value);
			}
		}

		private static UserDto? TryGetUserDto(ResolutionContext ctx, Guid userId)
		{
			// ctx.Items["UserDict"] kỳ vọng là Dictionary<Guid, UserDto>
			if (ctx.TryGetItems(out var items) &&
			    items.TryGetValue("UserDict", out var obj) &&
			    obj is Dictionary<Guid, UserDto> dict &&
			    dict.TryGetValue(userId, out var dto))
			{
				return dto;
			}
			return null;
		}
	}
}
