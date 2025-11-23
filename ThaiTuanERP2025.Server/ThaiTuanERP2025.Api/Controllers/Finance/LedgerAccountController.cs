using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Commands;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Queries;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Services;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
        [Authorize]
        [ApiController]
        [Route("api/ledger-account")]
        public class LedgerAccountController : ControllerBase
        {
                private readonly IMediator _mediator;
                public LedgerAccountController(IMediator mediator)
                {
                        _mediator = mediator;
                }

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
                {
                        var dtos = await _mediator.Send(new GetAllLedgerAccountsQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<LedgerAccountDto>>.Success(dtos));
                }

                [HttpGet("tree")]
                public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
                {
                        var tree = await _mediator.Send(new GetLedgerAccountTreeQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<LedgerAccountTreeDto>>.Success(tree));
                }

                [HttpPost]
                public async Task<IActionResult> Create([FromBody] LedgerAccountPayload payload, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new CreateLedgerAccountCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
