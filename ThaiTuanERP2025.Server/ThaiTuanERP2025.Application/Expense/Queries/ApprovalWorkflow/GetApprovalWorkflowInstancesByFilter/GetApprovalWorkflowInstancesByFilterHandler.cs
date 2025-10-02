using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstancesByFilter
{
	public sealed class GetApprovalWorkflowInstancesByFilterHandler : IRequestHandler<GetApprovalWorkflowInstancesByFilterQuery, IReadOnlyList<ApprovalWorkflowInstanceDto>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetApprovalWorkflowInstancesByFilterHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<ApprovalWorkflowInstanceDto>> Handle(GetApprovalWorkflowInstancesByFilterQuery query, CancellationToken cancellationToken) {
			var list = await _unitOfWork.ApprovalWorkflowInstances.ListByFilterAsync(
				query.DocumentType, query.DocumentId, query.Status, 
				query.BudgetCode, query.MinAmount, query.MaxAmount,
				cancellationToken
			);

			return _mapper.Map<List<ApprovalWorkflowInstanceDto>>( list );
		}
	}
}
