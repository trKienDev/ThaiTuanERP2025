using MediatR;
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
			var dto = await _expenseWorkflowTemplateRepo.GetByIdProjectedAsync(query.Id, cancellationToken)
				?? throw new DirectoryNotFoundException("Không tìm thấy luồng duyệt");
			return dto;
		}
	}
}
