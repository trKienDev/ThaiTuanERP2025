using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Commands
{
	public sealed record CreateExpenseWorkflowTemplateCommand(ExpenseWorkflowTemplatePayload Payload) : IRequest<Unit>;

	public sealed class CreateExpenseWorkflowTemplateCommandHandler : IRequestHandler<CreateExpenseWorkflowTemplateCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateExpenseWorkflowTemplateCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateExpenseWorkflowTemplateCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;
			var workflowTemplate = new ExpenseWorkflowTemplate(payload.Name, payload.Version);

			// steps
			if(payload.Steps is { Count: > 0 })
			{
				var ordered = payload.Steps.OrderBy(s => s.Order <= 0 ? int.MaxValue : s.Order).ToList();

				for (int i = 0; i < ordered.Count; i++)
				{
					var s = ordered[i];
					var flowType = Enum.Parse<ExpenseFlowType>(s.FlowType, ignoreCase: true);
					var approveMode = Enum.Parse<ExpenseApproveMode>(s.ApproveMode, ignoreCase: true);

					string? fixedApproversJson = null;
					string? resolverParamsJson = null;

					// Mode = Standard → yêu cầu ApproverIds
					if (approveMode == ExpenseApproveMode.Standard)
					{
						var ids = s.ApproverIds?.ToList() ?? new List<Guid>();
						if (ids.Count == 0)
							throw new ConflictException($"Bước {i + 1}: ApproverMode=standard yêu cầu ApproverIds.");

						fixedApproversJson = JsonSerializer.Serialize(ids);
					}
					else // Mode = Condition
					{
						if (string.IsNullOrWhiteSpace(s.ResolverKey))
							throw new ConflictException($"Bước {i + 1}: ApproverMode=condition yêu cầu ResolverKey.");

						if (s.ResolverParams != null)
							resolverParamsJson = JsonSerializer.Serialize(s.ResolverParams);
					}

					// SLA tối thiểu = 1
					var slaHours = s.SlaHours < 1 ? 1 : s.SlaHours;

					// Gán order tuần tự
					var finalOrder = i + 1;

					// Tạo step theo entity mới
					var step = new ExpenseStepTemplate(
						workflowTemplateId: workflowTemplate.Id,
						name: s.Name,
						order: finalOrder,
						flowType: flowType,
						slaHours: slaHours,
						 approveMode: approveMode,
						fixedApproverIdsJson: fixedApproversJson,
						resolverKey: s.ResolverKey,
						resolverParamsJson: resolverParamsJson
					);

					workflowTemplate.Steps.Add(step);
				}

				workflowTemplate.BumpVersion();
			}

			await _uow.ExpenseWorkflowTemplates.AddAsync(workflowTemplate);
			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
