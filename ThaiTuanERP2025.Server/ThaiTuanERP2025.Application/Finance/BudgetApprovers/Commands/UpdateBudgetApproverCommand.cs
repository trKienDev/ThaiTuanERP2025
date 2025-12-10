using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers.Commands
{
	public sealed record UpdateBudgetApproverRequest(Guid approverId, int slaHours, IEnumerable<Guid> DepartmentIds);
	public sealed record UpdateBudgetApproverCommand( Guid BudgetApproverId,  UpdateBudgetApproverRequest Request ) : IRequest<Unit>;

	public sealed class UpdateBudgetApproverCommandHandler : IRequestHandler<UpdateBudgetApproverCommand, Unit> {
		private readonly IUnitOfWork _uow;
		public UpdateBudgetApproverCommandHandler( IUnitOfWork uow )  {
			_uow = uow;	
		}

		public async Task<Unit> Handle(UpdateBudgetApproverCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var approver = await _uow.BudgetApprovers.SingleOrDefaultIncludingAsync(
				q => q.Id == command.BudgetApproverId,
				asNoTracking: false,
				cancellationToken: cancellationToken,
				includes: b => b.Departments
			);
			if (approver == null) throw new NotFoundException("Không tìm thấy user phê duyệt ngân sách");

			Guard.AgainstNullOrEmptyGuid(request.approverId, nameof(request.approverId));
			var user = await _uow.Users.ExistAsync(q => q.Id == request.approverId, cancellationToken);
			if (!user) throw new NotFoundException("Không tìm thấy user phê duyệt");

			if(request.approverId != approver.ApproverUserId) 
				approver.UpdateApprover(request.approverId, request.slaHours);	

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
