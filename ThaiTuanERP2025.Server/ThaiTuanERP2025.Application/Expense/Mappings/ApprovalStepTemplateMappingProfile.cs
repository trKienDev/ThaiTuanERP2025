// Application/Expense/Mappings/ApprovalStepTemplateMappingProfile.cs
using AutoMapper;
using System.Text.Json;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ApprovalStepTemplateMappingProfile : Profile
	{
		public ApprovalStepTemplateMappingProfile()
		{
			// Dùng TypeConverter để map sau khi EF đã materialize entity (in-memory)
			CreateMap<ApprovalStepTemplate, ApprovalStepTemplateDto>()
				.ConvertUsing<ApprovalStepTemplateToDtoConverter>();
		}
	}

	public sealed class ApprovalStepTemplateToDtoConverter : ITypeConverter<ApprovalStepTemplate, ApprovalStepTemplateDto>
	{
		public ApprovalStepTemplateDto Convert( ApprovalStepTemplate src, ApprovalStepTemplateDto dest, ResolutionContext ctx)
		{
			// 1) Tính sẵn các biến cần cho init
			Guid[]? approverIds = null;
			if (src.ApproverMode == ApproverMode.Standard && !string.IsNullOrWhiteSpace(src.FixedApproverIdsJson))
			{
				try
				{
					var raw = JsonSerializer.Deserialize<string[]>(src.FixedApproverIdsJson);
					approverIds = raw?.Select(Guid.Parse).ToArray();
				}
				catch
				{
					approverIds = Array.Empty<Guid>(); // hoặc để null theo policy của bạn
				}
			}

			object? resolverParams = null;
			if (src.ApproverMode == ApproverMode.Condition && !string.IsNullOrWhiteSpace(src.ResolverParamsJson))
			{
				try
				{
					resolverParams = JsonSerializer.Deserialize<object>(src.ResolverParamsJson);
				}
				catch
				{
					resolverParams = null;
				}
			}

			// 2) Trả về DTO bằng object initializer (hợp lệ với init-only)
			return new ApprovalStepTemplateDto
			{
				Id = src.Id,                                    // AuditableEntity.Id
				WorkflowTemplateId = src.WorkflowTemplateId,
				Name = src.Name,
				Order = src.Order,
				FlowType = src.FlowType == FlowType.OneOfN ? "OneOfN" : "Single",
				SlaHours = src.SlaHours,
				ApproverMode = src.ApproverMode == ApproverMode.Condition ? "Condition" : "Standard",

				// Standard
				ApproverIds = approverIds,

				// Condition
				ResolverKey = src.ApproverMode == ApproverMode.Condition ? src.ResolverKey : null,
				ResolverParams = resolverParams,
				AllowOverride = src.AllowOverride
			};
		}
	}
}
