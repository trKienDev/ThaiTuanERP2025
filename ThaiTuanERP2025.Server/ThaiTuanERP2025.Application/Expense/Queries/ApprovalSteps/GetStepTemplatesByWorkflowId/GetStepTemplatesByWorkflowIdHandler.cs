using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalSteps.GetStepTemplatesByWorkflowId
{
	public sealed class GetStepTemplatesByWorkflowIdHandler : IRequestHandler<GetStepTemplatesByWorkflowIdQuery, IReadOnlyList<ApprovalStepTemplateDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;	
		public GetStepTemplatesByWorkflowIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<ApprovalStepTemplateDto>> Handle(GetStepTemplatesByWorkflowIdQuery request, CancellationToken cancellationToken)
		{
			var entities = await _unitOfWork.ApprovalStepTemplates.ListAsync(q =>
				q.Where(x => !x.IsDeleted && x.IsDeleted == false && x.WorkflowTemplateId == request.WorkflowTemplateId)
				.OrderBy(x => x.Order),
				asNoTracking: true,
				cancellationToken: cancellationToken
			);

			return _mapper.Map<IReadOnlyList<ApprovalStepTemplateDto>>(entities);
		}
	}
}
