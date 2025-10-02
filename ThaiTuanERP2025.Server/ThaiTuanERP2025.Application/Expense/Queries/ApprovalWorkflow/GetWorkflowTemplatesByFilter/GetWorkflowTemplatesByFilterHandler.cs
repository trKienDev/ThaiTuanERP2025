using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetWorkflowTemplatesByFilter
{
	public sealed class GetWorkflowTemplatesByFilterHandler : IRequestHandler<GetWorkflowTemplatesByFilterQuery, IReadOnlyList<ApprovalWorkflowTemplateDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetWorkflowTemplatesByFilterHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<ApprovalWorkflowTemplateDto>> Handle(GetWorkflowTemplatesByFilterQuery query, CancellationToken cancellationToken = default)
		{
			var entities = await _unitOfWork.ApprovalWorkflowTemplates.ListByFilterAsync(query.DocumentType, query.IsActive, cancellationToken);
			return _mapper.Map<IReadOnlyList<ApprovalWorkflowTemplateDto>>(entities);
		}
	}
}
