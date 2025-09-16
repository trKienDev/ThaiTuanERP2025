using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Repositories;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetAllDepartments
{
	public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
	{
		private readonly IDepartmentRepository _departmentRepository;
		public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
		{
			_departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
		}

		public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
		{
			var departments = await _departmentRepository.GetAllAsync();
			return departments.Select(d => new DepartmentDto
			{
				Id = d.Id,
				Name = d.Name,
				Code = d.Code
			}).ToList();
		}
	}
}
