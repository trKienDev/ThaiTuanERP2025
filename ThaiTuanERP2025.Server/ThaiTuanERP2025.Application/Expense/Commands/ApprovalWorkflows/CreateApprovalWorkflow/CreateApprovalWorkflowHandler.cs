using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflow
{
	public class CreateApprovalWorkflowHandler : IRequestHandler<CreateApprovalWorkflowCommand, ApprovalWorkflowDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateApprovalWorkflowHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ApprovalWorkflowDto> Handle(CreateApprovalWorkflowCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var workflow = new ApprovalWorkflow
			{
				Name = request.Name,
				IsActive = request.IsActive,
			};

			foreach(var step in request.Steps.OrderBy(x => x.Order)) {
				workflow.AddStep(new ApprovalStep
				{
					Title = step.Title,
					Order = step.Order,
					FlowType = step.FlowType,
					SlaHours = step.SlaHours ?? 8,
					CandidateJson = JsonSerializer.Serialize(step.CandidateUserIds),
					Description = step.Description	
				});
			}

			return await _unitOfWork.ApprovalWorkflows.AddAndReturnDtoAsync(workflow, cancellationToken);
		}
	}
}
