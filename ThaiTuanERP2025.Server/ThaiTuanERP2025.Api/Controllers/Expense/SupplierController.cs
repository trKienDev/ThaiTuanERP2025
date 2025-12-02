using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Expense.Suppliers;
using ThaiTuanERP2025.Application.Expense.Suppliers.Commands;
using ThaiTuanERP2025.Application.Expense.Suppliers.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
        [ApiController]
        [Route("api/supplier")]
        [Authorize]
        public class SupplierController : ControllerBase
        {
                private readonly IMediator _mediator;
                public SupplierController(IMediator mediator)
                {
                    _mediator = mediator;
                }

                [HttpGet]
                public async Task<IActionResult> GetAll(CancellationToken cancellationToken )
                {
                        var dtos = await _mediator.Send(new GetAllSuppliersQuery(), cancellationToken);
                        return Ok(ApiResponse<IReadOnlyList<SupplierDto>>.Success(dtos));
                }

                [HttpGet("{id:guid}/beneficiary")]
                public async Task<IActionResult> GetBeneficiaryById([FromRoute] Guid id, CancellationToken cancellationToken)
                {
                        var dto = await _mediator.Send(new GetSupplierBeneficiaryInforQuery(id), cancellationToken);
                        return Ok(ApiResponse<SupplierBeneficiaryInforDto>.Success(dto));
                }

                [HttpPost]
                public async Task<IActionResult> Create([FromBody] CreateSupplierCommand command, CancellationToken cancellationToken) { 
                        var result = await _mediator.Send(command, cancellationToken);
                        return Ok(ApiResponse<Guid>.Success(result));
                }
        }
}
