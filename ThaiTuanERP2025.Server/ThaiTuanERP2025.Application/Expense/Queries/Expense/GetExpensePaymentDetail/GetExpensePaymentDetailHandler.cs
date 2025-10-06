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
			var payment = await _unitOfWork.ExpensePayments.GetDetailByIdAsync(query.Id, cancellationToken);
			if (payment == null)
				throw new NotFoundException($"ExpensePayment {query.Id} not found.");

			var workflow = await _unitOfWork.ExpensePayments.GetWorkflowInstanceAsync(payment.Id, cancellationToken);

			var dto = _mapper.Map<ExpensePaymentDetailDto>(payment) with
			{
				WorkflowInstance = workflow != null
					? _mapper.Map<ApprovalWorkflowInstanceDto>(workflow)
					: null
			};

			return dto;
		}
	}
}
