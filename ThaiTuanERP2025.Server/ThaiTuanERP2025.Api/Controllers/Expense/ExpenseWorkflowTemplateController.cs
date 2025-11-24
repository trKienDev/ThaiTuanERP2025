using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Commands;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
        [Authorize]
        [ApiController]
        [Route("api/expense-workflow-template")]
        public class ExpenseWorkflowTemplateController : ControllerBase
        {
                private readonly IMediator _mediator;
                public ExpenseWorkflowTemplateController(IMediator mediator)
                {
                        _mediator = mediator;
                }

                [HttpPost]
                public async Task<IActionResult> Create(ExpenseWorkflowTemplatePayload payload, CancellationToken cancellationToken)
                {
			var result = await _mediator.Send(new CreateExpenseWorkflowTemplateCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
