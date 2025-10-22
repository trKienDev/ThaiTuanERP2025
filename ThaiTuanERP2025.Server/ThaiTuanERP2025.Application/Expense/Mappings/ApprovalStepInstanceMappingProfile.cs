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
				.ForMember(d => d.DefaultApproverUser, o => o.MapFrom((s, d, destMember, ctx) => s.DefaultApproverId.HasValue ? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.DefaultApproverId.Value) : null))
				.ForMember(d => d.ApprovedByUser, o => o.MapFrom<ApprovedUserResolver<ApprovalStepInstanceDetailDto>>())
				.ForMember(d => d.RejectedByUser, o => o.MapFrom<RejectedUserResolver<ApprovalStepInstanceDetailDto>>())
				.ConvertUsing<StepInstanceDetailConverter>();


			CreateMap<ApprovalStepInstance, ApprovalStepInstanceStatusDto>()
				.ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.DefaultApproverUser, o => o.MapFrom((s, d, destMember, ctx) => s.DefaultApproverId.HasValue ? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.DefaultApproverId.Value) : null))
				.ForMember(d => d.ApprovedByUser, o => o.MapFrom<ApprovedUserResolver<ApprovalStepInstanceStatusDto>>())
				.ForMember(d => d.RejectedByUser, o => o.MapFrom<RejectedUserResolver<ApprovalStepInstanceStatusDto>>());
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

				var defaultApproverUser = s.DefaultApproverId.HasValue
				    ? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.DefaultApproverId.Value)
				    : null;

				return new ApprovalStepInstanceDetailDto
				{
					Id = s.Id,
					WorkflowInstanceId = s.WorkflowInstanceId,
					TemplateStepId = s.TemplateStepId,
					Name = s.Name,
					Order = s.Order,
					FlowType = s.FlowType == FlowType.OneOfN ? "OneOfN" : "Single",
					SlaHours = s.SlaHours,
					ApprovalMode = s.ApproverMode == ApproverMode.Condition ? "Condition" : "Standard",
					ResolvedApproverCandidateIds = candidates,
					ApproverCandidates = null, // sẽ fill sau
					DefaultApproverId = s.DefaultApproverId,
					SelectedApproverId = s.SelectedApproverId,
					Status = s.Status.ToString(),
					StartedAt = s.StartedAt,
					DueAt = s.DueAt,
					ApprovedAt = s.ApprovedAt,
					ApprovedBy = s.ApprovedBy,
					RejectedAt = s.RejectedAt,
					RejectedBy = s.RejectedBy,
					Comments = s.Comments,
					SlaBreached = s.SlaBreached,
					History = history,
					DefaultApproverUser = defaultApproverUser!,
				};
			}
		}

		private sealed class ApprovedUserResolver<TDestination> : IValueResolver<ApprovalStepInstance, TDestination, UserDto?>
		{
			public UserDto? Resolve(ApprovalStepInstance s, TDestination d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.ApprovedBy.HasValue) return null;
				return ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.ApprovedBy.Value);
			}
		}

		private sealed class RejectedUserResolver<TDestination> : IValueResolver<ApprovalStepInstance, TDestination, UserDto?>
		{
			public UserDto? Resolve(ApprovalStepInstance s, TDestination d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.RejectedBy.HasValue) return null;
				return ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.RejectedBy.Value);
			}
		}


		private sealed class DefaultApproverResolver : IValueResolver<ApprovalStepInstance, ApprovalStepInstanceStatusDto, UserDto?>
		{
			public UserDto? Resolve(ApprovalStepInstance s, ApprovalStepInstanceStatusDto d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.DefaultApproverId.HasValue) return null;
				return TryGetUserDto(ctx, s.DefaultApproverId.Value);
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
