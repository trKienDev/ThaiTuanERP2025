using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Queries.Expense.GetExpensePaymentDetail
{
	public sealed class GetExpensePaymentDetailHandler : IRequestHandler<GetExpensePaymentDetailQuery, ExpensePaymentDetailDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetExpensePaymentDetailHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ExpensePaymentDetailDto> Handle(GetExpensePaymentDetailQuery query, CancellationToken cancellationToken) {
			// 1) Load ExpensePayment + relations
			var payment = await _unitOfWork.ExpensePayments.SingleOrDefaultIncludingAsync(
				predicate: p => p.Id == request.Id && !p.IsDeleted,
					q => q.Include(p => p.CreatedByUser).ThenInclude(u => u.Department)
					.Include(p => p.Supplier)
					.Include(p => p.Items).ThenInclude(i => i.BudgetCode)
					.Include(p => p.Items).ThenInclude(i => i.CashOutCode)
					.Include(p => p.Attachments)
					.Include(p => p.Followers).ThenInclude(f => f.User)
					.Include(p => p.Invoices),
				asNoTracking: true,
				splitQuery: true,
				cancellationToken: cancellationToken
			);

			if(payment == null)
				throw new NotFoundException($"ExpensePayment {query.Id} not found.");

			// 2) Load WorkflowInstance
			var wfInstance = await _unitOfWork.ApprovalWorkflowInstances.SingleOrDefaultIncludingAsync(
				predicate: i => i.DocumentType == "ExpensePayment" && i.DocumentId == payment.Id && !i.IsDeleted,
				q => q.Include(i => i.Steps)
					.ThenInclude(s => s.Candidates)
					.ThenInclude(c => c.User),
				asNoTracking: true,
				splitQuery: true,
				cancellationToken: cancellationToken
			);

			// 3) Map bằng AutoMapper
			var dto = _mapper.Map<ExpensePaymentDetailDto>(payment);
			dto = dto with
			{
				WorkflowInstance = wfInstance != null ? _mapper.Map<ApprovalWorkflowInstanceDto>(wfInstance) : null
			};

			return dto;
		}
	}
}
