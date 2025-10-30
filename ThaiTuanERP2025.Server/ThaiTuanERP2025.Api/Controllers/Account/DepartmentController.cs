using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment;
using ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment;
using ThaiTuanERP2025.Application.Account.Commands.Departments.SetDepartmentManager;
using ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Queries.Departments.GetAllDepartments;

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

		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] AddDepartmentCommand command)
		{
			if (command == null)
				return BadRequest(ApiResponse<object>.Fail("Dữ liệu rộng hoặc không hợp lệ !"));

			var departmentId = await _mediator.Send(command);
			return Ok(ApiResponse<object>.Success( new {departmentId }));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentCommand body)
		{
			if (id != body.Id)
				return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));

			await _mediator.Send(body);
			return Ok(ApiResponse<string>.Success("Cập nhật department thành công"));
		}

		[HttpPut("{id:guid}/manager")]
		public async Task<IActionResult> SetManager([FromRoute] Guid id, [FromBody] SetDepartmentManagerCommand command)
		{
			if (command == null || command.DepartmentId == Guid.Empty || id != command.DepartmentId)
				return BadRequest(ApiResponse<string>.Fail("Dữ liệu rộng hoặc không hợp lệ !"));
			await _mediator.Send(command);
			return Ok(ApiResponse<string>.Success("Cập nhật quản lý phòng ban thành công"));
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
