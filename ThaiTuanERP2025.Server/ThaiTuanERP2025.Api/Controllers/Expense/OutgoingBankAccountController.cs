using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Commands;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
        [ApiController]
        [Route("api/outgoing-bank-account")]
        [Authorize]
        public class OutgoingBankAccountController : ControllerBase
        {
                private readonly IMediator _mediator;
                public OutgoingBankAccountController(IMediator mediator)
                {
                    _mediator = mediator;
                }

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new GetAllOutgoingBankAccountsQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<OutgoingBankAccountDto>>.Success(result));
                }

                [HttpPost]
                public async Task<IActionResult> Create([FromBody] OutgoingBankAccountPayload payload, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new CreateOutgoingBankAccountCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
		}
        }
}
