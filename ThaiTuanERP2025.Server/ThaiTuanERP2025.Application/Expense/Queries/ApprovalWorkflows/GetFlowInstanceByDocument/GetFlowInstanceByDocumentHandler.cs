using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetFlowInstanceByDocument
{
	public sealed class GetFlowInstanceByDocumentHandler : IRequestHandler<GetFlowInstanceByDocumentQuery, FlowInstanceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetFlowInstanceByDocumentHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<FlowInstanceDto> Handle(GetFlowInstanceByDocumentQuery query, CancellationToken cancellationToken) {
			// 1 ) Flow + steps
			var flow = await _unitOfWork.ApprovalFlowInstances.GetByDocumentWithStepsAsync(query.DocumentType, query.DocumentId, cancellationToken)
				?? throw new NotFoundException("Approval flow instance not found for this document");
			var steps = flow.Steps.OrderBy(s => s.OrderIndex).ToList();

			// 2 ) Actions cho tất cả các step
			var stepIds = steps.Select(s => s.Id).ToArray();
			var actions = await _unitOfWork.ApprovalActions.ListByStepIdsAsync(stepIds, cancellationToken);
			var lookup = actions.GroupBy(a => a.StepInstanceId).ToDictionary(g => g.Key, g => g.ToList());

			// 3 ) Map DTO
			return new FlowInstanceDto
			{
				Id = flow.Id,
				DocumentType = flow.DocumentType,
				DocumentId = flow.DocumentId,
				Status = flow.Status.ToString(),
				StartedAt = flow.StartedAt,
				FinishedAt = flow.FinishedAt,
				Steps = steps.Select(s =>
				{
					IReadOnlyList<Guid> candidates = string.IsNullOrWhiteSpace(s.CandidatesJson) ? Array.Empty<Guid>() : JsonSerializer.Deserialize<List<Guid>>(s.CandidatesJson) ?? new List<Guid>();
					var stepActions = lookup.TryGetValue(s.Id, out var list) ? list : new List<ApprovalAction>();

					return new FlowStepDto
					{
						Id = s.Id,
						Name = s.Name,
						OrderIndex = s.OrderIndex,
						Status = s.Status.ToString(),
						ApprovedByUserId = s.ApprovedByUserId,
						StartedAt = s.StartedAt,
						FinishedAt = s.FinishedAt,
						Candidates = candidates,
						Actions = stepActions.Select(a => new StepActionDto
						{
							Id = a.Id,
							ActorUserId = a.ActorUserId,
							Action = a.Action.ToString(),
							Comment = a.Comment,
							OccuredAt = a.OccuredAt,
							AttachmentFileIds = string.IsNullOrWhiteSpace(a.AttachmentFileIdsJson) ? null : JsonSerializer.Deserialize<List<Guid>>(a.AttachmentFileIdsJson)
						}).ToList()
					};
				}).ToList()
			};
		}
	}
}
