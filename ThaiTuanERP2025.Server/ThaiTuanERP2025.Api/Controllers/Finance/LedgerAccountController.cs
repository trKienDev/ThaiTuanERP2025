using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Commands;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Queries;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Services;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
        [Authorize]
        [ApiController]
        [Route("api/ledger-account")]
        public class LedgerAccountController : ControllerBase
        {
                private readonly IMediator _mediator;
                private readonly ILedgerAccountExcelReader _ledgerAccountExcelReader;
                public LedgerAccountController(IMediator mediator, ILedgerAccountExcelReader ledgerAccountExcelReader)
                {
                        _mediator = mediator;
                        _ledgerAccountExcelReader = ledgerAccountExcelReader;
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

                [HttpPost("excel")]
                public async Task<IActionResult> ImportExcel(IFormFile file, CancellationToken cancellationToken)
                {
                        if (file == null || file.Length == 0)
                                throw new BusinessRuleViolationException("File trống.");

                        using var stream = file.OpenReadStream();
                        var rows = _ledgerAccountExcelReader.Read(stream);

                        var result = await _mediator.Send(new BulkCreateLedgerAccountsCommand(rows), cancellationToken);

                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
