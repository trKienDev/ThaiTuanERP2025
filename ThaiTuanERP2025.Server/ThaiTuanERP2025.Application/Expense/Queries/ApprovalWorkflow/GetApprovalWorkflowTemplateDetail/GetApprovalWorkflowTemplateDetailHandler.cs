using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowTemplateDetail
{
	public sealed class GetApprovalWorkflowTemplateDetailHandler : IRequestHandler<GetApprovalWorkflowTemplateDetailQuery, ApprovalWorkflowTemplateDetailDto>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public GetApprovalWorkflowTemplateDetailHandler(IMapper mapper, IUnitOfWork unitOfWork) {
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<ApprovalWorkflowTemplateDetailDto> Handle(GetApprovalWorkflowTemplateDetailQuery query, CancellationToken cancellationToken) {
			var template = await _unitOfWork.ApprovalWorkflowTemplates.GetByIdAsync(query.Id)
				?? throw new NotFoundException("Không tìm thấy template");

			var steps = await _unitOfWork.ApprovalStepTemplates.ListAsync(
				src => src.Where(s => !s.IsDeleted && s.WorkflowTemplateId == query.Id)
						.OrderBy(s => s.Order),
				asNoTracking: true,
				cancellationToken: cancellationToken
			);

			return new ApprovalWorkflowTemplateDetailDto
			{
				Template = _mapper.Map<ApprovalWorkflowTemplateDto>(template),
				Steps = _mapper.Map<List<ApprovalStepTemplateDto>>(steps)
			};
		}
	}
}
