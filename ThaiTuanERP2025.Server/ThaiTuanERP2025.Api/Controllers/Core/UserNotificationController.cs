using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using ThaiTuanERP2025.Application.Core.Notifications.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Core
{
	[ApiController]
	[Route("api/notification")]
	[Authorize]
	public class UserNotificationController : ControllerBase
	{
		private readonly IMediator _mediator;
		public UserNotificationController(IMediator mediator) => _mediator = mediator;

		[HttpGet]
		public async Task<IActionResult> GetList(
			[FromQuery] bool unreadOnly = false,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 30,
			CancellationToken cancellationToken = default
		)
		{
			var result = await _mediator.Send(new GetAllNotificationsQuery(unreadOnly, page, pageSize), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<UserNotificationDto>>.Success(result));
		}

		[HttpGet("unread-count")]
		public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
		{
			var count = await _mediator.Send(new GetUnreadNotificationCountQuery(), cancellationToken);
			return Ok(ApiResponse<int>.Success(count));
		}
	}
}
