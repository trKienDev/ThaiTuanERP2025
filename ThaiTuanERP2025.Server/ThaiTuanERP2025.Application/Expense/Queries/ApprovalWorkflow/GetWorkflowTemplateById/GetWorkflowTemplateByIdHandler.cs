using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetWorkflowTemplateById
{
	public class GetWorkflowTemplateByIdHandler : IRequestHandler<GetWorkflowTemplateByIdQuery, ApprovalWorkflowTemplateDto?>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetWorkflowTemplateByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflowTemplateDto?> Handle(GetWorkflowTemplateByIdQuery query, CancellationToken cancellationToken = default)
		{
			var entity = await _unitOfWork.ApprovalWorkflowTemplates.GetByIdAsync(query.Id);
			return _mapper.Map<ApprovalWorkflowTemplateDto?>(entity);
		}
	}
}
