using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers.Commands
{
	public sealed record UpdateBudgetApproverDepartmentRequest(IEnumerable<Guid> DepartmentIds);
	public sealed record UpdateBudgetApproverDepartmentCommand(
		Guid BudgetApproverId, 
		UpdateBudgetApproverDepartmentRequest Request
	) : IRequest<Unit>;

	public sealed class UpdateBudgetApproverDepartmentCommandHandler : IRequestHandler<UpdateBudgetApproverDepartmentCommand, Unit> {
		private readonly IUnitOfWork _uow;
		public UpdateBudgetApproverDepartmentCommandHandler( IUnitOfWork uow )  {
			_uow = uow;	
		}

		public async Task<Unit> Handle(UpdateBudgetApproverDepartmentCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var approver = await _uow.BudgetApprovers.GetByIdAsync(command.BudgetApproverId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy user phê duyệt ngân sách");

			var currentDeptIds = approver.Departments.Select(d => d.DepartmentId).ToHashSet();
			var newDeptIds = request.DepartmentIds.ToHashSet();

			var addedDeptIds = newDeptIds.Except(currentDeptIds).ToList();
			foreach (var deptId in addedDeptIds)
			{
				approver.AssignToDepartment(deptId);
			}

			var removedDeptIds = currentDeptIds.Except(newDeptIds).ToList();
			foreach (var deptId in removedDeptIds)
			{
				approver.RemoveFromDepartment(deptId);
			}

			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
