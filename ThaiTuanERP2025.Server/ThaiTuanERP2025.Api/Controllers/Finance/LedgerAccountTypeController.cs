using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Commands;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
        [ApiController]
        [Authorize]
        [Route("api/ledger-account-type")]
        public class LedgerAccountTypeController : ControllerBase
        {
                private readonly IMediator _mediator;
                public LedgerAccountTypeController(IMediator mediator)
                {
                        _mediator = mediator;
                }

                [HttpPost] 
                public async Task<IActionResult> Create([FromBody] LedgerAccountTypePayload payload, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new CreateLedgerAccountTypeCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
