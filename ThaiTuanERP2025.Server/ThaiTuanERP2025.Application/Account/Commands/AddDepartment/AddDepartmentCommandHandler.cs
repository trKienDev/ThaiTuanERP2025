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
	public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, Guid>
	{
		private readonly IDepartmentRepository _departmentRepository;
		public AddDepartmentCommandHandler(IDepartmentRepository departmentRepository)
		{
			_departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
		}

		public async Task<Guid> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
		{
			var department = new Department(request.Name, request.Code);
			await _departmentRepository.AddAsync(department, cancellationToken);
			return department.Id;
		}
	}	
}
