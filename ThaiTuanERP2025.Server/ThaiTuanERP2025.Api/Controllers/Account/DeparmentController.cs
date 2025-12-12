using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Departments.Commands;
using ThaiTuanERP2025.Application.Account.Departments.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[ApiController]
	[Route("api/department")]
	[Authorize]
	public class DeparmentController : ControllerBase
	{
		private readonly IMediator _mediator;
		public DeparmentController(IMediator mediator) => _mediator = mediator;

		//[HttpGet]
		//public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		//{
		//	var deptDtos = await _mediator.Send(new GetAllDepartmentsQuery(), cancellationToken);
		//	return Ok(ApiResponse<IReadOnlyList<DepartmentDto>>.Success(deptDtos));
		//}

		[HttpGet("{id:guid}/parent")]
		public async Task<IActionResult> GetParentDepartment([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			var parentDeptDto = await _mediator.Send(new GetParentDepartmentQuery(id), cancellationToken);
			return Ok(ApiResponse<DepartmentDto?>.Success(parentDeptDto));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command, CancellationToken cancellation)
		{
			if (command == null)
				return BadRequest(ApiResponse<object>.Fail("Dữ liệu rộng hoặc không hợp lệ !"));

			var result = await _mediator.Send(command, cancellation);
			return Ok(ApiResponse<object>.Success(result));
		}

		[HttpPut("{id:guid}/parent")]
		public async Task<IActionResult> SetParent([FromRoute] Guid id , [FromRoute] Guid parentDept, CancellationToken cancellation)
		{
			var result = await _mediator.Send(new SetParentDepartmentCommand(id, parentDept), cancellation);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPut("{id:guid}/managers")]
		public async Task<IActionResult> AssignManagers([FromRoute] Guid id, [FromBody] SetDepartmentManagersRequest request, CancellationToken cancellation) {
			var result = await _mediator.Send(new SetDepartmentManagerCommand(id, request), cancellation);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
