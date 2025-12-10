using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Commands;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Queries;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Services;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
        [ApiController]
        [Authorize]
        [Route("api/ledger-account-type")]
        public class LedgerAccountTypeController : ControllerBase
        {
                private readonly IMediator _mediator;
                private readonly ILedgerAccountTypeExcelReader _excelReader;
                public LedgerAccountTypeController(IMediator mediator, ILedgerAccountTypeExcelReader excelReader)
                {
                        _mediator = mediator;
                        _excelReader = excelReader;
                }

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new GetAllLedgerAccountTypesQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<LedgerAccountTypeDto>>.Success(result));
                }

                [HttpPost] 
                public async Task<IActionResult> Create([FromBody] LedgerAccountTypePayload payload, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new CreateLedgerAccountTypeCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }

                [HttpPost("excel")]
                public async Task<IActionResult> Import(IFormFile file, CancellationToken cancellationToken)
                {
                        if (file == null || file.Length == 0)
                        {
                                return BadRequest(ApiResponse<Unit>.Fail("File không hợp lệ hoặc trống."));
                        }

                        // 1. đọc excel (Infrastructure service)
                        using var stream = file.OpenReadStream();
                        var rows = _excelReader.Read(stream);

                        if (rows == null || rows.Count == 0)
                                return BadRequest(ApiResponse<Unit>.Fail("Không tìm thấy dữ liệu trong file."));

                        // 2. gọi command (Application)
                        var command = new BulkCreateLedgerAccountTypesCommand(rows);
                        var result = await _mediator.Send(command, cancellationToken);

                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
