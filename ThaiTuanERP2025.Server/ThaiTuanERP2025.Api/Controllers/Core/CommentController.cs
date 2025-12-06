using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Core.Comments.Commands;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Application.Core.Comments.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Core
{
        [ApiController]
        [Route("api/comment")]
        [Authorize]
        public class CommentController : ControllerBase
        {
                private readonly IMediator _mediator;
                public CommentController(IMediator mediator) => _mediator = mediator;

                [HttpGet]
		public async Task<IActionResult> GetComments([FromQuery] string module, [FromQuery] string entity, [FromQuery] Guid entityId, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(entity) || entityId == Guid.Empty)
				return BadRequest(ApiResponse<object>.Fail("module, entity, entityId là bắt buộc"));

			var query = new GetCommentsQuery(module, entity, entityId);

			var result = await _mediator.Send(query, cancellationToken);

			return Ok(ApiResponse<IReadOnlyList<CommentDetailDto>>.Success(result));
		}

		[HttpPost]
                public async Task<IActionResult> Create([FromBody] CommentPayload payload, CancellationToken cancellationToken)
                {
                        if (payload is null)
                                return BadRequest(ApiResponse<object>.Fail("Dữ liệu không hợp lệ"));

                        var result = await _mediator.Send(new AddCommentCommand(payload), cancellationToken);
                        return Ok(ApiResponse<CommentDetailDto>.Success(result));
                }
        }
}
