using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.UpdateApprovalWorkflow
{
	public class UpdateApprovalWorkflowHandler : IRequestHandler<UpdateApprovalWorkflowCommand, ApprovalWorkflowDto?>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UpdateApprovalWorkflowHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflowDto?> Handle(UpdateApprovalWorkflowCommand command,  CancellationToken cancellationToken) {
			var request = command.Request;
			// 1 ) Lấy workflow kèm Steps
			var workflow = await _unitOfWork.ApprovalWorkflows.SingleOrDefaultIncludingAsync(command.Id, cancellationToken);
			if (workflow is null) return null;

			// 2 ) Update fields cơ bản
			workflow.Name = request.Name?.Trim() ?? workflow.Name;
			workflow.IsActive = request.IsActive ?? workflow.IsActive;

			// 3 ) Replace-all steps: xoá toàn bộ & thêm lại theo payload
			workflow.Steps.Clear();
			foreach(var step in request.Steps.OrderBy(s => s.Order))
			{
				workflow.Steps.Add(new ApprovalStep
				{
					Id = Guid.NewGuid(),
					Title = step.Title.Trim(),
					Order = step.Order,
					FlowType = step.FlowType,
					SlaHours = step.SlaHours ?? 8,
					CandidateJson = System.Text.Json.JsonSerializer.Serialize(step.CandidateUserIds.ToArray())
				});
			}

			// 4 ) Lưu
			_unitOfWork.ApprovalWorkflows.Update(workflow);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// 5 ) Materialize lại để map DTO (đảm bảo Step đã cập nhật)
			var materialized = await _unitOfWork.ApprovalWorkflows.SingleOrDefaultIncludingAsync(command.Id, cancellationToken);
			return _mapper.Map<ApprovalWorkflowDto>(materialized);
		}
	}
}
