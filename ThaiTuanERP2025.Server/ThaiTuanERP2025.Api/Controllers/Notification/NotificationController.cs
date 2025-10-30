using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Notifications.Command.Notifications.MarkAllRead;
using ThaiTuanERP2025.Application.Notifications.Command.Notifications.MarkRead;
using ThaiTuanERP2025.Application.Notifications.Dtos;
using ThaiTuanERP2025.Application.Notifications.Queries.Notifications.GetAllNotifications;
using ThaiTuanERP2025.Application.Notifications.Queries.Notifications.GetUnreadCount;

namespace ThaiTuanERP2025.Api.Controllers.Notification
{
	[ApiController]
	[Route("api/notification")]
	[Authorize]
	public sealed class NotificationController : ControllerBase
	{
		private readonly IMediator _mediator;
		public NotificationController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<ApiResponse<IReadOnlyCollection<AppNotificationDto>>>> GetList(
			[FromQuery] bool unreadOnly = false,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 30,
			CancellationToken cancellationToken = default
		) {
			var result = await _mediator.Send(new GetAllNotificationsQuery(unreadOnly, page, pageSize), cancellationToken);
			return Ok(ApiResponse<IReadOnlyCollection<AppNotificationDto>>.Success(result));
		}

		[HttpGet("unread-count")]
		public async Task<ActionResult<ApiResponse<int>>> GetUnreadCount(CancellationToken cancellationToken)
		{
			var count = await _mediator.Send(new GetUnreadCountQuery(), cancellationToken);
			return Ok(ApiResponse<int>.Success(count));
		}

		[HttpPost("{id:guid}/read")]
		public async Task<ActionResult<ApiResponse<Unit>>> MarkRead(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new MarkReadCommand(id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("read-all")]
		public async Task<ActionResult<ApiResponse<Unit>>> MarkAllRead(CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new MarkAllReadCommand(), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
