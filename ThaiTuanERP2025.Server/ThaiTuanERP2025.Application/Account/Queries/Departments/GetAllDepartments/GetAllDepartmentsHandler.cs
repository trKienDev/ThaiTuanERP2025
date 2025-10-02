using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Queries.Departments.GetAllDepartments
{
	public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllDepartmentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
		{
			return await _unitOfWork.Departments.ListProjectedAsync<DepartmentDto>(
				q => q.ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider),
				asNoTracking: true,
				cancellationToken: cancellationToken
			);
		}
	}
}
