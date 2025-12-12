//using AutoMapper;
//using MediatR;
//using ThaiTuanERP2025.Application.Core.FileAttachments.Repositories;

//namespace ThaiTuanERP2025.Application.Account.Departments.Queries
//{
//	public record GetAllDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDto>>;
//	public sealed class GetAllDepartmentQueryHandler : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<DepartmentDto>>
//	{
//		private readonly IDepartmentReadRepository _deptReadRepo;
//		private readonly IFileAttachmentReadRepository _fileAttachmentRepo;
//		private readonly IMapper _mapper;
//		public GetAllDepartmentQueryHandler(IDepartmentReadRepository deptReadRepo, IMapper mapper, IFileAttachmentReadRepository storedFileRepo)
//		{
//			_deptReadRepo = deptReadRepo;
//			_mapper = mapper;
//			_fileAttachmentRepo = storedFileRepo;
//		}

//		public async Task<IReadOnlyList<DepartmentDto>> Handle(GetAllDepartmentsQuery query, CancellationToken cancellationToken)
//		{
//			var departments = await _deptReadRepo.ListWithManagersAsync(cancellationToken);

//			if (departments.Count == 0)
//				return Array.Empty<DepartmentDto>();

//			var avatarIds = departments
//				.SelectMany(d => d.Managers.Select(m => m.User.AvatarFileId))
//				.Where(id => id.HasValue)
//				.Select(id => id!.Value)
//				.Distinct()
//				.ToList();

//			var avatarDict = avatarIds.Count > 0
//				? await _storedFileRepo.GetObjectKeysByIdsAsync(avatarIds, cancellationToken)
//				: new Dictionary<Guid, string>();

//			var departmentDtos = _mapper.Map<IReadOnlyList<DepartmentDto>>(departments, opt =>
//			{
//				opt.Items["AvatarDict"] = avatarDict;
//			});

//			return departmentDtos;
//		}
//	}
//}
