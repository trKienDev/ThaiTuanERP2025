using MediatR;
using ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Queries
{
	public sealed record GetExpenseWorkflowTemplateByIdQuery(Guid Id) : IRequest<ExpenseWorkflowTemplateDto?>;
	public sealed class GetExpenseWorkflowTemplateByIdQueryHanlder : IRequestHandler<GetExpenseWorkflowTemplateByIdQuery, ExpenseWorkflowTemplateDto?>
	{
		private readonly IExpenseWorkflowTemplateReadRepository _expenseWorkflowTemplateRepo;
		public GetExpenseWorkflowTemplateByIdQueryHanlder(IExpenseWorkflowTemplateReadRepository expenseWorkflowTemplateRepo)
		{
			_expenseWorkflowTemplateRepo = expenseWorkflowTemplateRepo;
		}

		public async Task<ExpenseWorkflowTemplateDto?> Handle(GetExpenseWorkflowTemplateByIdQuery query, CancellationToken cancellationToken)
		{
			var workflow = await _expenseWorkflowTemplateRepo.GetByIdProjectedAsync(query.Id, cancellationToken)
				?? throw new DirectoryNotFoundException("Không tìm thấy luồng duyệt");

                        if (workflow.Steps is List<ExpenseStepTemplateDto> list)
                        {
                                list.Sort((a, b) => a.Order.CompareTo(b.Order));
                        }

                        return workflow;
		}
	}
}
