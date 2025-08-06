using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment
{
	public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AddDepartmentCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
		{
			var department = new Department(request.Name, request.Code);
			await _unitOfWork.Departments.AddAsync(department);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
