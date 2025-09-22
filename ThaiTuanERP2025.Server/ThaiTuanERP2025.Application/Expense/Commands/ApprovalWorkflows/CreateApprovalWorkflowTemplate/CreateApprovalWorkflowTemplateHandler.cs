using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowTemplate
{
	public sealed class CreateApprovalWorkflowTemplateHandler : IRequestHandler<CreateApprovalWorkflowTemplateCommand, ApprovalWorkflowTemplateDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateApprovalWorkflowTemplateHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflowTemplateDto> Handle(CreateApprovalWorkflowTemplateCommand command, CancellationToken cancellationToken = default) {
			var request = command.Request;

			// Nếu tạo template active, enforce rule: (DocumentType, Scope) chỉ có 1 active
			if (request.IsActive) {
				var exists = await _unitOfWork.ApprovalWorkflowTemplates.ExistsActiveForScopeAsync(
					request.DocumentType, cancellationToken
				);
				if (exists)
					throw new ConflictException("Luồng duyệt này đã tồn tại");
			}

			var entity = new ApprovalWorkflowTemplate(request.Name, request.DocumentType);

			if (request.IsActive)
				entity.Activate();

			await _unitOfWork.ApprovalWorkflowTemplates.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ApprovalWorkflowTemplateDto>(entity);
		}
	}
}
