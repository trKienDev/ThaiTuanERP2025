using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment;
using ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment;
using ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Departments.GetAllDepartments;
using ThaiTuanERP2025.Application.Account.Queries.Departments.GetDepartmentsByIds;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class DepartmentController : ControllerBase
	{
		private readonly IMediator _mediator;
		public DepartmentController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpGet("all")]
		public async Task<ActionResult<List<DepartmentDto>>> GetAll(CancellationToken cancellationToken)
		{
			var departments = await _mediator.Send(new GetAllDepartmentsQuery(), cancellationToken);
			return Ok(ApiResponse<List<DepartmentDto>>.Success(departments));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] AddDepartmentCommand command)
		{
			if (command == null)
				return BadRequest(ApiResponse<object>.Fail("Dữ liệu rộng hoặc không hợp lệ !"));

			var departmentId = await _mediator.Send(command);
			return Ok(ApiResponse<object>.Success( new {departmentId }));
		}

		[HttpPost("by-ids")]
		public async Task<ActionResult<List<DepartmentDto>>> GetByIds([FromBody] List<Guid> ids, CancellationToken cancellationToken)
		{
			if(ids == null || !ids.Any())
				return BadRequest(ApiResponse<List<DepartmentDto>>.Fail("Dữ liệu rộng hoặc không hợp lệ !"));
			var departments = await _mediator.Send(new GetDepartmentsByIdsQuery(ids), cancellationToken);
			return Ok(ApiResponse<List<DepartmentDto>>.Success(departments));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentCommand body)
		{
			if (id != body.Id)
				return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));

			await _mediator.Send(body);
			return Ok(ApiResponse<string>.Success("Cập nhật department thành công"));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			if(id == Guid.Empty)
				return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));
			await _mediator.Send(new DeleteDepartmentCommand(id));
			return Ok(ApiResponse<string>.Success("Xoá department thành công"));
		}
    }
}
