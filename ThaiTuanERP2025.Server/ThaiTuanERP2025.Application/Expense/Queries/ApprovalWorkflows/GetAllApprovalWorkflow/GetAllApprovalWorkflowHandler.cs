using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetAllApprovalWorkflow
{
	public class GetAllApprovalWorkflowHandler : IRequestHandler<GetAllApprovalWorkflowQuery, IReadOnlyList<ApprovalWorkflowDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllApprovalWorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<ApprovalWorkflowDto>> Handle(GetAllApprovalWorkflowQuery request, CancellationToken cancellationToken)
		{
			var workflows = await _unitOfWork.ApprovalWorkflows.ListAllIncludingAsync(cancellationToken);
			return _mapper.Map<IReadOnlyList<ApprovalWorkflowDto>>(workflows);
		}
	}
}
