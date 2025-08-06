using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetDepartmentsByIds
{
	public class GetDepartmentsByIdsQueryHandler : IRequestHandler<GetDepartmentsByIdsQuery, List<DepartmentDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetDepartmentsByIdsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<List<DepartmentDto>> Handle(GetDepartmentsByIdsQuery request, CancellationToken cancellationToken)
		{
			if (request.DepartmentIds == null || !request.DepartmentIds.Any())
			{
				return new List<DepartmentDto>();
			}
			var departments = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentIds, cancellationToken);
			return departments.Select(d => new DepartmentDto
			{
				Id = d.Id,
				Name = d.Name,
				Code = d.Code
			}).ToList();
		}
	}
}
