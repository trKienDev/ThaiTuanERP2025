using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.Divisions.CreateDivision;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Divisions.GetAllDivisions;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class DivisionController : ControllerBase
	{
		private readonly IMediator _mediator;
		public DivisionController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}
		
		[HttpGet("all")]
		public async Task<ActionResult<List<DivisionSummaryDto>>> GetAll(CancellationToken cancellationToken) {
			var summaryDto = await _mediator.Send(new GetAllDivisionsQuery(), cancellationToken);
			return Ok(ApiResponse<List<DivisionSummaryDto>>.Success(summaryDto));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateDivisionCommand command)
		{
			if(command == null) 
				return BadRequest(ApiResponse<object>.Fail("Dữ liệu rộng hoặc không hợp lệ !"));

			var divisionId = await _mediator.Send(command);
			return Ok(ApiResponse<object>.Success(new { divisionId }));
		}
	}
}
