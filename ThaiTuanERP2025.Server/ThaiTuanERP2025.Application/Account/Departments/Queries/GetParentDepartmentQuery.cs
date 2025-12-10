using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Departments.Queries
{
	public sealed record GetParentDepartmentQuery(Guid DeptId) : IRequest<DepartmentDto?>;
	public sealed class GetParentDepartmentQueryHandler : IRequestHandler<GetParentDepartmentQuery, DepartmentDto?>
	{
		private readonly IDepartmentReadRepository _deptReadRepo;
		public GetParentDepartmentQueryHandler(IDepartmentReadRepository deptReadRepo) => _deptReadRepo = deptReadRepo;

		public async Task<DepartmentDto?> Handle(GetParentDepartmentQuery query, CancellationToken cancellationToken)
		{
			var childDept = await _deptReadRepo.GetByIdProjectedAsync(query.DeptId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy phòng ban yêu cầu");

			if (!childDept.ParentId.HasValue) return null;

			var parentId = childDept.ParentId.Value;
			var parentDeptDto = await _deptReadRepo.GetByIdProjectedAsync(parentId, cancellationToken);
			return parentDeptDto;
		}
	}

}
