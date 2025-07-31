using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.AddDepartment
{
	public class BulkAddDepartmentsCommandHandler : IRequestHandler<BulkAddDepartmentsCommand, int>
	{
		private readonly IDepartmentRepository _departmentRepository;
		public BulkAddDepartmentsCommandHandler(IDepartmentRepository departmentRepository)
		{
			_departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
		}

		public async Task<int> Handle(BulkAddDepartmentsCommand request, CancellationToken cancellationToken)
		{
			int count = 0;
			foreach(var dto in request.Departments)
			{
				var dept = new Department(dto.Name, dto.Code);
				await _departmentRepository.AddAsync(dept, cancellationToken);
				count++;
			}
			return count;
		}
	}
}
