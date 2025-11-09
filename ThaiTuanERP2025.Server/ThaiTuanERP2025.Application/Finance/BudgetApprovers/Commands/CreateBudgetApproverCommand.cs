using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers.Commands
{
	public sealed record CreateBudgetApproverCommand(
		Guid ApproverId,
		int SlaHours,
		List<Guid> DepartmentIds
	) : IRequest<Unit>;

	public sealed class CreateBudgetApproverCommandHandller : IRequestHandler<CreateBudgetApproverCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateBudgetApproverCommandHandller(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;	

		public async Task<Unit> Handle(CreateBudgetApproverCommand command, CancellationToken cancellationToken) {
			Guard.AgainstNullOrEmptyGuid(command.ApproverId, nameof(command.ApproverId));	
			Guard.AgainstNegativeOrZero(command.SlaHours, nameof(command.SlaHours));	
			foreach(var deptId in command.DepartmentIds)  
				Guard.AgainstNullOrEmptyGuid(deptId, nameof(deptId) );

			var approver = await _unitOfWork.Users.ExistAsync(q => q.Id == command.ApproverId, cancellationToken);
			if (approver is false) throw new NotFoundException("Không tìm thấy user phê duyệt");

			var validDeptCount = await _unitOfWork.Departments.CountAsync(
				d => command.DepartmentIds.Contains(d.Id),
				cancellationToken
			);
			if (validDeptCount != command.DepartmentIds.Count)
				throw new NotFoundException("Một hoặc nhiều phòng ban không hợp lệ");

			if (await _unitOfWork.BudgetApprovers.ExistAsync(a => a.ApproverUserId == command.ApproverId, cancellationToken))
				throw new ConflictException("User này đã là người phê duyệt ngân sách.");

			var budgetApprover = new BudgetApprover(command.ApproverId, command.SlaHours);
			foreach(var deptId in command.DepartmentIds) 
				budgetApprover.AssignToDepartment(deptId);

			await _unitOfWork.BudgetApprovers.AddAsync(budgetApprover);
			await _unitOfWork.SaveChangesAsync();
			return Unit.Value;
		}
	}
}

