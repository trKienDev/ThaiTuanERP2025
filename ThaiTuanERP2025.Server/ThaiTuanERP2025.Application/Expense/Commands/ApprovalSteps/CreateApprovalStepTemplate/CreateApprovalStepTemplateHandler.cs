using AutoMapper;
using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.CreateApprovalStepTemplate
{
	public sealed class CreateApprovalStepTemplateHandler : IRequestHandler<CreateApprovalStepTemplateCommand, ApprovalStepTemplateDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateApprovalStepTemplateHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalStepTemplateDto> Handle(CreateApprovalStepTemplateCommand command,  CancellationToken cancellationToken) {
			var request = command.Request;

			// 1 ) Check template exists	
			var template = await _unitOfWork.ApprovalWorkflowTemplates.GetByIdAsync(request.WorkflowTemplateId)
				?? throw new NotFoundException("Không tìm thấy luồng duyệt") ;

			// 2 ) Unique (WorkflowTemplateId, Order)
			if (await _unitOfWork.ApprovalStepTemplates.ExistOrderAsync(request.WorkflowTemplateId, request.Order, null, cancellationToken))
			{
				throw new ConflictException("Thứ tự đã tồn tại trong luồng duyệt");
			}

			// 3 ) Build entity
			var flow = request.FlowType == "OneOfN" ? FlowType.OneOfN : FlowType.Single;
			var mode = request.ApproverMode == "Condition" ? ApproverMode.Condition : ApproverMode.Standard;

			string? fixedApproversJson = null;
			string? resolverParamsJson = null;

			if(mode == ApproverMode.Standard)
			{
				if (request.ApproverIds == null || request.ApproverIds.Length == 0)
					throw new ConflictException("Danh sách người duyệt không được để trống");

				// serialize GUIDs
				fixedApproversJson = JsonSerializer.Serialize(request.ApproverIds.Select(x => x.ToString("D")).ToArray());
			}
			else
			{ // Condition
				if (string.IsNullOrWhiteSpace(request.ResolverKey))
					throw new ConflictException("ResolverKey không được để trống");
				if (request.ResolverParams is not null)
					resolverParamsJson = JsonSerializer.Serialize(request.ResolverParams);
			}

			var entity = new ApprovalStepTemplate(
				workflowTemplateId: request.WorkflowTemplateId,
				name: request.Name,
				order: request.Order,
				flowType: flow,
				slaHours: request.SlaHours,
				approverMode: mode,
				fixedApproverIdsJson: fixedApproversJson,
				resolverKey: request.ResolverKey,
				resolverParamsJson: resolverParamsJson,
				allowOverride: request.AllowOverride
			);

			await _unitOfWork.ApprovalStepTemplates.AddAsync(entity);

			// bump version on template
			template.BumpVersion();
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ApprovalStepTemplateDto>(entity);
		}
	}
}
