using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Alerts.TaskReminders;
using ThaiTuanERP2025.Application.Alerts.TaskReminders.Commands.DismissReminder;
using ThaiTuanERP2025.Application.Alerts.TaskReminders.Queries.GetMyReminders;

namespace ThaiTuanERP2025.Api.Controllers.Notification
{
	[ApiController]
	[Route("api/task-reminder")]
	[Authorize]
	public class TaskReminderController : Controller
	{
		private readonly IMediator _mediator;
		public TaskReminderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<ApiResponse<IReadOnlyCollection<TaskReminderDto>>>> Get(CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetMyRemindersQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<TaskReminderDto>>.Success(result));
		}

		[HttpPost("{id:guid}/dismiss")]
		public async Task<ActionResult<ApiResponse<Unit>>> Dismiss(Guid id, CancellationToken ct)
		{
			var result = await _mediator.Send(new DismissReminderCommand(id), ct);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
