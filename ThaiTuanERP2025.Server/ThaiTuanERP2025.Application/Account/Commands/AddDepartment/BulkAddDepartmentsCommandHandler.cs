using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.AddDepartment
{
	public class BulkAddDepartmentsCommandHandler : IRequestHandler<BulkAddDepartmentsCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public BulkAddDepartmentsCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(BulkAddDepartmentsCommand request, CancellationToken cancellationToken)
		{
			var departments = new List<Department>();
			foreach(var dto in request.Departments) {
				var dept = new Department(dto.Name, dto.Code);
				departments.Add(dept);
			}

			await _unitOfWork.Departments.AddRangeAysnc(departments);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
