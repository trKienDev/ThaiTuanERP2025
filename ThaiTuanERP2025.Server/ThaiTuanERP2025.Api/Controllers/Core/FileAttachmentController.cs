using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Core.FileAttachments.Commands;

namespace ThaiTuanERP2025.Api.Controllers.Core
{
        [ApiController]
        [Route("api/file")]
        [Authorize]
        public class FileAttachmentController : ControllerBase
        {
                private readonly IMediator _mediator;
                public FileAttachmentController(IMediator mediator)
                {
                    _mediator = mediator;
                }

                [HttpPost] 
                public async Task<IActionResult> Create([FromBody] CreateFileAttachmentCommand command, CancellationToken cancellationToken)
                {
                        if (command is null)
                                return BadRequest(ApiResponse<string>.Fail("Dữ liệu không hợp lệ"));

                        var result = await _mediator.Send(command, cancellationToken);
                        return Ok(ApiResponse<Guid>.Success(result));
                }
        }
}
