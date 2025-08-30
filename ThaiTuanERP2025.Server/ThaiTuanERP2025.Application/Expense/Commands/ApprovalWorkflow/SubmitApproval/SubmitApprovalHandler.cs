using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Services.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.SubmitApproval
{
	public class SubmitApprovalHandler : IRequestHandler<SubmitApprovalCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private IApproverResolverService _approverResolverService;
		public SubmitApprovalHandler(IUnitOfWork unitOfWork, IApproverResolverService approverResolverService)
		{
			_unitOfWork = unitOfWork;
			_approverResolverService = approverResolverService;
		}

		public async Task<Guid> Handle(SubmitApprovalCommand command, CancellationToken cancellationToken) {
			var latestActiveFlowDefinition = await _unitOfWork.ApprovalFlowDefinitions.GetLastestActiveByDocumentTypeAsync(command.DocumentType, cancellationToken);
			if (latestActiveFlowDefinition == null)
				throw new InvalidOperationException($"No active approval flow for {command.DocumentType}");

			var flowInstance = new ApprovalFlowInstance
			{
				Id = Guid.NewGuid(),
				FlowDefinitionId = latestActiveFlowDefinition.Id,
				DefinitionVersion = latestActiveFlowDefinition.Version,
				DocumentType = command.DocumentType,
				DocumentId = command.DocumentId,
				Status = ApprovalFlowStatus.InProgress,
				StartedAt = DateTime.UtcNow,
			};

			foreach (var stepDefinition in latestActiveFlowDefinition.Steps)
			{
				var candidates = _approverResolverService.Resolve(stepDefinition, command.SelectedApproverId);
				if (candidates is null || !candidates.Any())
					throw new InvalidOperationException($"Step '{stepDefinition.Name}' has noi candidates");

				flowInstance.Steps.Add(new ApprovalStepInstance
				{
					Id = Guid.NewGuid(),
					FlowInstanceId = flowInstance.Id,
					StepDefinitionId = stepDefinition.Id,
					Name = stepDefinition.Name,
					OrderIndex = stepDefinition.OrderIndex,
					Status = ApprovalStepStatus.Pending,
					CandidatesJson = JsonSerializer.Serialize(candidates),
					RequiredCount = stepDefinition.RequiredCount,
					ApprovedCount = 0,
				});
			}

			var firstStep = flowInstance.Steps.OrderBy(s => s.OrderIndex).First();
			firstStep.Status = ApprovalStepStatus.InProgress;
			firstStep.StartedAt = DateTime.UtcNow;
			// nếu bạn cần Deadline theo SLA: cần SlaHours từ StepDefinition → clone sang StepInstance khi tạo
			// (nếu đã có, set first.DeadlineAt = StartedAt + SlaHours)

			await _unitOfWork.ApprovalFlowInstances.AddAsync(flowInstance);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return flowInstance.Id;
		}
	}
}
