using System.Text.Json;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Factories
{
	public interface IExpenseWorkflowFactory
	{
		Task<ExpenseWorkflowInstance> CreateForExpensePaymentAsync(ExpensePayment payment, CancellationToken cancellationToken = default);
	}

	public sealed class ExpenseWorkflowFactory : IExpenseWorkflowFactory
	{
		private readonly IUnitOfWork _uow;
		public ExpenseWorkflowFactory(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<ExpenseWorkflowInstance> CreateForExpensePaymentAsync(ExpensePayment payment, CancellationToken cancellationToken = default)
		{
			// 1 ) Load workflow template đang active
			var template = await _uow.ExpenseWorkflowTemplates.SingleOrDefaultIncludingAsync(
				predicate: t => t.IsActive,
				includes: t => t.Steps,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy luồng duyệt");

			// 2 ) Sort steps để đảm bảo thứ tự
			var orderedSteps = template.Steps.OrderBy(s => s.Order).ToList();
			if(!orderedSteps.Any())
				throw new BusinessRuleViolationException("Workflow template không chứa bước phê duyệt nào.");

			// 3 ) Create workflow instance
			var workflowInstance = new ExpenseWorkflowInstance(
				    templateId: template.Id,
				    templateVersion: template.Version,
				    documentType: DocumentType.ExpensePayment,
				    documentId: payment.Id
			);

			// 4 ) Generate step instances
			foreach (var stepTemplate in orderedSteps)
			{
				string approversJson = string.Empty;

				// Nếu step duyệt cố định → copy approver list
				if (stepTemplate.ApproveMode == ExpenseApproveMode.Standard && !string.IsNullOrWhiteSpace(stepTemplate.FixedApproverIdsJson))
				{
					approversJson = stepTemplate.FixedApproverIdsJson;
				} else // Duyệt có điều kiện
				{
					if(stepTemplate.ResolverKey == ExpenseStepResolverKey.DepartmentManager)
					{
						approversJson = JsonSerializer.Serialize( new List<Guid> { payment.ManagerApproverId } );
					}
				}

				var stepInstance = new ExpenseStepInstance(
					workflowInstanceId: workflowInstance.Id,
					stepTemplateId: stepTemplate.Id,
					name: stepTemplate.Name,
					order: stepTemplate.Order,
					flowType: stepTemplate.FlowType,
					slaHours: stepTemplate.SlaHours,
					approverMode: stepTemplate.ApproveMode,
					approversJson: approversJson,
					status: StepStatus.Pending
				);

				workflowInstance.Steps.Add(stepInstance);
			}

			return workflowInstance;
		}
	}
}
