using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Account.Departments.Queries
{
	public record GetAllDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDto>>;
	public sealed class GetAllDepartmentQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<DepartmentDto>>
	{
		private readonly IDepartmentReadRepository _deptReadRepo;
		private readonly IMapper _mapper;
		public GetAllDepartmentQueryHandler(IDepartmentReadRepository deptReadRepo, IMapper mapper)
		{
			_deptReadRepo = deptReadRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<DepartmentDto>> Handle(GetAllDepartmentsQuery query, CancellationToken cancellationToken)
		{
			return await _deptReadRepo.ListProjectedAsync(
				q => q.Where(d => d.IsActive)
					.OrderByDescending(q => q.Level)
					.ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}

}
