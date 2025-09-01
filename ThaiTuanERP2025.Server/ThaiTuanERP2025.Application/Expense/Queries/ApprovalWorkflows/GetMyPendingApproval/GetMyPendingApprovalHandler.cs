using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetMyPendingApproval
{
	public sealed class GetMyPendingApprovalHandler : IRequestHandler<GetMyPendingApprovalQuery, IReadOnlyList<MyPendingApprovalDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public GetMyPendingApprovalHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<IReadOnlyList<MyPendingApprovalDto>> Handle(GetMyPendingApprovalQuery query, CancellationToken cancellationToken) {
			var actor = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Không xác định được user hiện tại");

			// 1 ) Lấy tất cả các step đang InProgress kèm flow
			var rows = await _unitOfWork.ApprovalFlowInstances.ListInProgressStepsWithFlowAsync(cancellationToken);

			// 2 ) Lọc theo CandidatesJson chứa actor
			var list = new List<MyPendingApprovalDto>(capacity: rows.Count);
			foreach(StepWithFlow x in rows) {
				var candidates = ParseCandidates(x.Step.CandidatesJson);
				if (!candidates.Contains(actor)) continue;

				list.Add(new MyPendingApprovalDto
				{
					StepInstanceId = x.Step.Id,
					FlowInstanceId = x.Step.FlowInstanceId,	
					DocumentType = x.Flow.DocumentType,
					DocumentId = x.Flow.DocumentId,
					StepName = x.Step.Name,
					OrderIndex = x.Step.OrderIndex,
					StartedAt = x.Step.StartedAt,
					DeadlineAt = x.Step.DeadlineAt,
				});
			}

			// 3 ) (tùy chọn) sắp xếp UI-friendly: gần deadline trước
			return list.OrderBy(d => d.DeadlineAt ?? DateTime.MaxValue)
				.ThenBy(d => d.StartedAt ?? DateTime.MinValue)
				.ToList();
		}

		private static IReadOnlyList<Guid> ParseCandidates(string? json) {
			if(string.IsNullOrWhiteSpace(json))
				return Array.Empty<Guid>();
			return JsonSerializer.Deserialize<List<Guid>>(json) ?? (IReadOnlyList<Guid>) Array.Empty<Guid>();
		}
	}
}
