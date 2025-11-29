using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Commands;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
        [ApiController]
        [Authorize]
        [Route("api/expense-workflow-instance")]
        public class ExpenseWorkflowInstanceController : ControllerBase
        {
                private readonly IMediator _mediator;
                public ExpenseWorkflowInstanceController(IMediator mediator)
                {
                        _mediator = mediator;
                }

                [HttpPost("approve/{id:guid}")]
                public async Task<IActionResult> Approve([FromRoute] Guid id, CancellationToken cancellationToken)
                {
                        var result = await _mediator.Send(new ApproveExpenseStepInstanceCommand(id), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
