using AutoMapper;
using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowTemplate
{
	public sealed class CreateApprovalWorkflowTemplateHandler : IRequestHandler<CreateApprovalWorkflowTemplateCommand, ApprovalWorkflowTemplateDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateApprovalWorkflowTemplateHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflowTemplateDto> Handle(CreateApprovalWorkflowTemplateCommand command, CancellationToken cancellationToken = default) {
			var request = command.Request;

			// 1 ) Tạo & lưu template ==> Id
			var entity = new ApprovalWorkflowTemplate(request.Name);
			//await _unitOfWork.ApprovalWorkflowTemplates.AddAsync(entity);
			//await _unitOfWork.SaveChangesAsync();

			// 2 ) Thêm steps
			if(request.Steps is { Count: > 0}) {
				// Chuẩn hóa thứ tự:  1..N theo thứ tự gửi lên
				var ordered = request.Steps.OrderBy(s => s.Order <= 0 ? int.MaxValue : s.Order).ToList();

				for(var i = 0; i < ordered.Count; i++) {
					var s = ordered[i];
					var flowType = ParseFlowType(s.FlowType);
					var approverMode = ParseApproverMode(s.ApproverMode);

					// validate theo mode
					string? approverIdsJson = null;
					string? resolverParamsJson = null;

					if(approverMode == ApproverMode.Standard) {
						var ids = s.ApproverIds?.ToList() ?? new List<Guid>();
						if (ids.Count == 0)
							throw new ConflictException($"Bước {i + 1}: ApproverMode=standard yêu cầu ApproverIds.");
						approverIdsJson = JsonSerializer.Serialize(ids);
					} else {
						if (string.IsNullOrWhiteSpace(s.ResolverKey))
							throw new ConflictException($"Bước {i + 1}: ApproverMode=condition yêu cầu ResolverKey.");
						if (s.ResolverParams is not null)
							resolverParamsJson = JsonSerializer.Serialize(s.ResolverParams);
					}

					// SLA tối thiểu 1 (theo form FE)
					var sla = s.SlaHours < 1 ? 1 : s.SlaHours;

					// Gán order tuần tự từ 1..N
					var order = i + 1;

					// Tạo step và add trực tiếp vào navigation (không cần AddStep method)
					entity.Steps.Add(new ApprovalStepTemplate(
						workflowTemplateId: entity.Id,              // cần Id đã có
						name: s.Name,
						order: order,
						flowType: flowType,
						slaHours: sla,
						approverMode: approverMode,
						fixedApproverIdsJson: approverIdsJson,
						resolverKey: s.ResolverKey,
						resolverParamsJson: resolverParamsJson,
						allowOverride: s.AllowOverride
					));
				}

				// bump version duy nhất 1 lần sau khi thay đổi cấu trúc steps
				entity.BumpVersion();
			}

			await _unitOfWork.ApprovalWorkflowTemplates.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<ApprovalWorkflowTemplateDto>(entity);
		}

		private static ExpenseFlowType ParseFlowType(string value) =>
			value?.Trim().ToLowerInvariant() switch {
				"single" => ExpenseFlowType.Single,
				"one-of-n" => ExpenseFlowType.OneOfN,
				_ => throw new ConflictException($"FlowType không hợp lệ: {value}")
			};

		private static ApproverMode ParseApproverMode(string value) =>
			value?.Trim().ToLowerInvariant() switch {
				"standard" => ApproverMode.Standard,
				"condition" => ApproverMode.Condition,
				_ => throw new ConflictException($"ApproverMode không hợp lệ: {value} ")
			};
	}
}
