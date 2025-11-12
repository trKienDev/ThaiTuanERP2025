using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Core.Reminders.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Core
{
	[ApiController]
	[Route("api/reminder")]
	[Authorize]
	public class UserReminderController : ControllerBase
	{
		private readonly IMediator _mediator;
		public UserReminderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetMyRemindersQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<UserReminderDto>>.Success(result));
		}

	}
}
