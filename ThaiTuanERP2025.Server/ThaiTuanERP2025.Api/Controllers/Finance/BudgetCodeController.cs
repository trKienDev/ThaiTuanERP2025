using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.UpdateBudgetCodeStatus;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllActiveBudgetCodes;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllBudgetCodes;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Route("api/budget-code")]
	[ApiController]
	public class BudgetCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public BudgetCodeController(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mediator = mediator;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllBudgetCodesQuery());
			return Ok(ApiResponse<List<BudgetCodeDto>>.Success(result));
		}

		[HttpGet("active")]
		public async Task<IActionResult> GetAllActive() {
			var codes = await _mediator.Send(new GetAllActiveBudgetCodesQuery());
			return Ok(ApiResponse<List<BudgetCodeDto>>.Success(codes));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBudgetCodeCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetCodeDto>.Success(result));
		}

		[HttpPut("{id}/status")]
		public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] bool isActive)
		{
			var command = new UpdateBudgetCodeStatusCommand { Id = id, IsActive = isActive };
			await _mediator.Send(command);
			return Ok(ApiResponse<bool>.Success(true));
		}
	}
}
