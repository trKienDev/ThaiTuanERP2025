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
			CreateMap<ExpenseStepInstance, ApprovalStepInstanceDto>()
				.ConvertUsing<StepInstanceConverter>();

			CreateMap<ExpenseStepInstance, ApprovalStepInstanceDetailDto>()
				.ConvertUsing<StepInstanceDetailConverter>();


			CreateMap<ExpenseStepInstance, ApprovalStepInstanceStatusDto>()
				.ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.DefaultApproverUser, o => o.MapFrom((s, d, destMember, ctx) => s.DefaultApproverId.HasValue ? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.DefaultApproverId.Value) : null))
				.ForMember(d => d.ApprovedByUser, o => o.MapFrom<ApprovedUserResolver<ApprovalStepInstanceStatusDto>>())
				.ForMember(d => d.RejectedByUser, o => o.MapFrom<RejectedUserResolver<ApprovalStepInstanceStatusDto>>());
		}

		private sealed class StepInstanceConverter : ITypeConverter<ExpenseStepInstance, ApprovalStepInstanceDto>
		{
			public ApprovalStepInstanceDto Convert(ExpenseStepInstance s, ApprovalStepInstanceDto d, ResolutionContext ctx)
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
					s.FlowType == ExpenseFlowType.OneOfN ? "OneOfN" : "Single",
					s.SlaHours,
					s.ApproverMode == ApproverMode.Condition ? "Condition" : "Standard",
					candidates,
					s.DefaultApproverId,
					s.SelectedApproverId,
					s.Status.ToString(),
					AsUtc(s.StartedAt),
					 AsUtc(s.DueAt),
					 AsUtc(s.ApprovedAt),
					s.ApprovedBy,
					AsUtc(s.RejectedAt),
					s.RejectedBy,
					s.Comments,
					s.SlaBreached,
					history
				);
			}
		}

		private sealed class StepInstanceDetailConverter : ITypeConverter<ExpenseStepInstance, ApprovalStepInstanceDetailDto>
		{
			public ApprovalStepInstanceDetailDto Convert(ExpenseStepInstance s, ApprovalStepInstanceDetailDto d, ResolutionContext ctx)
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

				var defaultApproverUser = s.DefaultApproverId.HasValue? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.DefaultApproverId.Value) : null;
				var approvedByUser = s.ApprovedBy.HasValue ? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.ApprovedBy.Value) : null;
				var rejectedByUser = s.RejectedBy.HasValue ? ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.RejectedBy.Value) : null;

				return new ApprovalStepInstanceDetailDto
				{
					Id = s.Id,
					WorkflowInstanceId = s.WorkflowInstanceId,
					TemplateStepId = s.TemplateStepId,
					Name = s.Name,
					Order = s.Order,
					FlowType = s.FlowType == ExpenseFlowType.OneOfN ? "OneOfN" : "Single",
					SlaHours = s.SlaHours,
					ApprovalMode = s.ApproverMode == ApproverMode.Condition ? "Condition" : "Standard",
					ResolvedApproverCandidateIds = candidates,
					ApproverCandidates = null, // sẽ fill sau
					DefaultApproverId = s.DefaultApproverId,
					SelectedApproverId = s.SelectedApproverId,
					Status = s.Status.ToString(),
					StartedAt = AsUtc(s.StartedAt),
					DueAt = AsUtc(s.DueAt),
					ApprovedAt = AsUtc(s.ApprovedAt),
					ApprovedBy = s.ApprovedBy,
					ApprovedByUser = approvedByUser,
					RejectedAt = AsUtc(s.RejectedAt),
					RejectedBy = s.RejectedBy,
					RejectedByUser = rejectedByUser,
					Comments = s.Comments,
					SlaBreached = s.SlaBreached,
					History = history,
					DefaultApproverUser = defaultApproverUser!,
				};
			}
		}

		private sealed class ApprovedUserResolver<TDestination> : IValueResolver<ExpenseStepInstance, TDestination, UserDto?>
		{
			public UserDto? Resolve(ExpenseStepInstance s, TDestination d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.ApprovedBy.HasValue) return null;
				return ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.ApprovedBy.Value);
			}
		}

		private sealed class RejectedUserResolver<TDestination> : IValueResolver<ExpenseStepInstance, TDestination, UserDto?>
		{
			public UserDto? Resolve(ExpenseStepInstance s, TDestination d, UserDto? destMember, ResolutionContext ctx)
			{
				if (!s.RejectedBy.HasValue) return null;
				return ApprovalStepInstanceMappingProfile.TryGetUserDto(ctx, s.RejectedBy.Value);
			}
		}

		private sealed class DefaultApproverResolver : IValueResolver<ExpenseStepInstance, ApprovalStepInstanceStatusDto, UserDto?>
		{
			public UserDto? Resolve(ExpenseStepInstance s, ApprovalStepInstanceStatusDto d, UserDto? destMember, ResolutionContext ctx)
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

		private static DateTime? AsUtc(DateTime? dt)
		{
			if (!dt.HasValue) return null;
			// Gắn Kind=Utc, không đổi ticks
			return DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
		}

	}
}
