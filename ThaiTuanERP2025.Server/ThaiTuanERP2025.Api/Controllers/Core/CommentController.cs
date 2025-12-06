using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Core.Comments.Commands;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Application.Core.Comments.Queries;
using ThaiTuanERP2025.Domain.Shared.Enums;

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
		public async Task<IActionResult> GetComments([FromQuery] DocumentType documentType, [FromQuery] Guid documentId, CancellationToken cancellationToken)
		{
			if (documentId == Guid.Empty)
				return BadRequest(ApiResponse<object>.Fail("module, entity, entityId là bắt buộc"));

			var query = new GetCommentsQuery(documentType, documentId);

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
