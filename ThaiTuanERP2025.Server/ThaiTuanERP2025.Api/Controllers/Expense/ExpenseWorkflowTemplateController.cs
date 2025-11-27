using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Commands;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Queries;

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

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
                {
                        var dtos = await _mediator.Send(new GetAllExpenseWorfklowTemplatesQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<ExpenseWorkflowTemplateDto>>.Success(dtos)); 
                }

                [HttpGet("{id:guid}")]
                public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken )
                {
                        var dto = await _mediator.Send(new GetExpenseWorkflowTemplateByIdQuery(id), cancellationToken);
                        return Ok(ApiResponse<ExpenseWorkflowTemplateDto>.Success(dto!));
                }

                [HttpPost]
                public async Task<IActionResult> Create(ExpenseWorkflowTemplatePayload payload, CancellationToken cancellationToken)
                {
			var result = await _mediator.Send(new CreateExpenseWorkflowTemplateCommand(payload), cancellationToken);
                        return Ok(ApiResponse<Unit>.Success(result));
                }
        }
}
