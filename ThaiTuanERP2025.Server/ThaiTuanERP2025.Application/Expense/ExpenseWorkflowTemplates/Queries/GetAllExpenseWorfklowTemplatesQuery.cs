using MediatR;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Queries
{
	public sealed record GetAllExpenseWorfklowTemplatesQuery() : IRequest<IReadOnlyList<ExpenseWorkflowTemplateDto>>;

	public sealed class GetAllExpenseWorfklowTemplatesQueryHandler : IRequestHandler<GetAllExpenseWorfklowTemplatesQuery, IReadOnlyList<ExpenseWorkflowTemplateDto>>
	{
		private readonly IExpenseWorkflowTemplateReadRepository _expenseWorkflowTemplateRepo;
		public GetAllExpenseWorfklowTemplatesQueryHandler(IExpenseWorkflowTemplateReadRepository expenseWorkflowTemplateRepo) { 
			_expenseWorkflowTemplateRepo = expenseWorkflowTemplateRepo;
		}

		public async Task<IReadOnlyList<ExpenseWorkflowTemplateDto>> Handle(GetAllExpenseWorfklowTemplatesQuery query, CancellationToken cancellationToken)
		{
			 return await _expenseWorkflowTemplateRepo.ListAsync(
				q => q.Where(x => x.IsActive),
				cancellationToken: cancellationToken
			);
		}
	}
}
