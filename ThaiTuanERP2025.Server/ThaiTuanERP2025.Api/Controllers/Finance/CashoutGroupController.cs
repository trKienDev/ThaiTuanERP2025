using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Commands;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
        [ApiController]
        [Authorize]
        [Route("api/cashout-group")]
        public class CashoutGroupController : ControllerBase
        {
                private readonly IMediator _mediator;
                public CashoutGroupController(IMediator mediator)
                {
                        _mediator = mediator;
                }

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
                {
                        var dtos = await _mediator.Send(new GetAllCashoutGroupsQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<CashoutGroupDto>>.Success(dtos));
                }

                [HttpGet("tree")]
                public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
                {
                        var trees = await _mediator.Send(new GetCashoutGroupsTreeQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<CashoutGroupTreeDto>>.Success(trees));
                }
 
                [HttpPost]
                public async Task<IActionResult> Create([FromBody] CashoutGroupPayload payload, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new CreateCashoutGroupCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
