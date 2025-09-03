using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetApprovalWorkflowById
{
	public sealed class GetApprovalWorkflowByIdHandler : IRequestHandler<GetApprovalWorkflowByIdQuery, ApprovalWorkflowDto?>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetApprovalWorkflowByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflowDto?> Handle(GetApprovalWorkflowByIdQuery request, CancellationToken cancellationToken)
		{
			var workflow = await _unitOfWork.ApprovalWorkflows.GetByIdAsync(request.Id);
			if (workflow is null) return null;

			return _mapper.Map<ApprovalWorkflowDto>(workflow);
		}
	}
}
