using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstanceDetail
{
	public sealed class GetApprovalWorkflowInstanceDetailHandler : IRequestHandler<GetApprovalWorkflowInstanceDetailQuery, ApprovalWorkflowInstanceDetailDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetApprovalWorkflowInstanceDetailHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflowInstanceDetailDto> Handle(GetApprovalWorkflowInstanceDetailQuery query, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.ApprovalWorkflowInstances.SingleOrDefaultIncludingAsync(
				x => x.Id == query.Id,
				asNoTracking: true,
				cancellationToken: cancellationToken,
				x => x.Steps
			);

			if (entity == null)
				throw new DirectoryNotFoundException("Không tìm thấy workflow instance");

			var dto = _mapper.Map<ApprovalWorkflowInstanceDto>(entity);
			var steps = dto.Steps.OrderBy(s => s.Order).ToList();

			return new ApprovalWorkflowInstanceDetailDto
			{
				WorkflowInstance = dto,
				Steps = steps
			};
		}
	}
}
