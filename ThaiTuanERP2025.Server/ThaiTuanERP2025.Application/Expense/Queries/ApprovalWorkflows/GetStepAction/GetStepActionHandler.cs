using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetStepAction
{
	public sealed class GetStepActionHandler : IRequestHandler<GetStepActionQuery, IReadOnlyList<StepActionDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetStepActionHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<IReadOnlyList<StepActionDto>> Handle(GetStepActionQuery query, CancellationToken cancellationToken) {
			var items = await _unitOfWork.ApprovalActions.ListByStepAsync(query.StepInstanceId, cancellationToken);

			var result = items.Select(a => new StepActionDto
			{
				Id = a.Id,
				StepInstanceId = a.StepInstanceId,
				ActorUserId = a.ActorUserId,
				Action = a.Action.ToString(),
				Comment = a.Comment,
				OccuredAt = a.OccuredAt,
				AttachmentFileIds = string.IsNullOrWhiteSpace(a.AttachmentFileIdsJson) ? null : JsonSerializer.Deserialize<List<Guid>>(a.AttachmentFileIdsJson)
			}).ToList();

			return result;
		}
	}
}
