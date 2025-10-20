using AutoMapper;
using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Contracts.Resolvers;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowInstance
{
	public sealed class CreateApprovalWorkflowInstanceHandler : IRequestHandler<CreateApprovalWorkflowInstanceCommand, ApprovalWorkflowInstanceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IApproverResolverRegistry? _resolverRegistry;	
		public CreateApprovalWorkflowInstanceHandler(IUnitOfWork unitOfWork, IMapper mapper, IApproverResolverRegistry? resolverRegistry)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_resolverRegistry = resolverRegistry;
		}

		public async Task<ApprovalWorkflowInstanceDto> Handle(CreateApprovalWorkflowInstanceCommand command, CancellationToken cancellationToken) {
			var request = command.Request;

			// 1 ) Load template + steps
			var template = await _unitOfWork.ApprovalWorkflowTemplates.GetByIdAsync(request.TemplateId)
				?? throw new NotFoundException("Không tìm thấy luồng duyệt");
			var steps = await _unitOfWork.ApprovalStepTemplates.ListAsync(q =>
				q.Where(s => s.WorkflowTemplateId == request.TemplateId && !s.IsDeleted)
				.OrderBy(s => s.Order),
				asNoTracking: true,
				cancellationToken: cancellationToken
			);
			
			if (steps.Count == 0)
				throw new ConflictException("Luồng phê duyệt không có bước duyệt nào");

			// 2 ) Build instance root
			var instance = new ApprovalWorkflowInstance(
				templateId: template.Id,
				    templateVersion: template.Version,        // <-- nhớ dùng version
				    documentType: request.DocumentType,
				    documentId: request.DocumentId,
				    createdByUserId: request.CreatorId,
				    amount: request.Amount,
				    currency: request.Currency,
				    budgetCode: request.BudgetCode,
				    costCenter: request.CostCenter,
				    rawJson: null
			);

			// 3 ) Resolve each sterp to StepInstance
			var context = new ResolverContext(request.CreatorId, request.Amount ?? 0m, request.BudgetCode, request.CostCenter, request.DocumentType);
			foreach (var s in steps)
			{
				string? candidatesJson = null;
				Guid? defaultApprover = null;
				Guid? selectedApprover = null;

				if (s.ApproverMode == ApproverMode.Standard)
				{
					// Use FixedApproverIdsJson from template
					candidatesJson = s.FixedApproverIdsJson;
					if (!string.IsNullOrWhiteSpace(candidatesJson))
					{
						try
						{
							var ids = JsonSerializer.Deserialize<string[]>(candidatesJson);
							defaultApprover = ids?.FirstOrDefault() is string first ? Guid.Parse(first) : null;
							selectedApprover = defaultApprover; // lock by default for single; UI có thể override sau nếu policy khác
						}
						catch { }
					}
				}
				else // Condition
				{
					if (_resolverRegistry is null)
						throw new InvalidOperationException("Resolver registry is not configured.");

					var resolver = _resolverRegistry.Get(s.ResolverKey ?? "")
						      ?? throw new InvalidOperationException($"Resolver '{s.ResolverKey}' not found.");

					var (candiates, @default) = await resolver.ResolveAsync(context, cancellationToken);
					candidatesJson = JsonSerializer.Serialize(candiates.Select(x => x.ToString("D")).ToArray());
					defaultApprover = @default;
					// nếu AllowOverride=false thì khóa selected = default; nếu true thì để default, UI có thể đổi trước khi submit
					selectedApprover = s.AllowOverride ? @default : @default;
				}

				var stepInstance = new ApprovalStepInstance(
					workflowInstanceId: instance.Id,
					templateStepId: s.Id,
					name: s.Name,
					order: s.Order,
					flowType: s.FlowType,
					slaHours: s.SlaHours,
					approverMode: s.ApproverMode,
					candidatesJson: candidatesJson,
					defaultApproverId: defaultApprover,
					selectedApproverId: selectedApprover,
					status: StepStatus.Pending
				);

				instance.Steps.Add(stepInstance);
			}

			// 4) Persist
			await _unitOfWork.ApprovalWorkflowInstances.AddAsync(instance);
			await _unitOfWork.SaveChangesAsync();

			// 5) Optionally start
			if (request.StartImmediately)
			{
				var first = instance.Steps.OrderBy(x => x.Order).First();
				first.Activate(DateTime.UtcNow);
				instance.Start(first.Order);
				await _unitOfWork.SaveChangesAsync();
			}

			// 6) Map out
			// materialize instance + steps (đã có sẵn trong memory) → map
			return _mapper.Map<ApprovalWorkflowInstanceDto>(instance);
		}
	}
}
