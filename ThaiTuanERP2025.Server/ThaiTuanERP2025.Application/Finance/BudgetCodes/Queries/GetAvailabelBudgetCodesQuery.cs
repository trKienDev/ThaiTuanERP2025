//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using MediatR;
//using System.Net.WebSockets;
//using ThaiTuanERP2025.Application.Account.Departments;
//using ThaiTuanERP2025.Application.Account.Users;
//using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
//using ThaiTuanERP2025.Application.Finance.BudgetPlans;
//using ThaiTuanERP2025.Application.Shared.Exceptions;
//using ThaiTuanERP2025.Application.Shared.Interfaces;

//namespace ThaiTuanERP2025.Application.Finance.BudgetCodes.Queries
//{
//	/// <summary>
//	/// Lấy những mã ngân sách chưa có kế hoạch ngân sách (filter theo phòng ban / budgetPeriod đang mở)
//	/// </summary>
//	public sealed record GetAvailabelBudgetCodesQuery : IRequest<IReadOnlyList<BudgetCodeLookupDto>>;

//	public sealed class GetAvailabelBudgetCodesQueryHandler : IRequestHandler<GetAvailabelBudgetCodesQuery, IReadOnlyList<BudgetCodeLookupDto>>
//	{
//		private readonly IBudgetCodeReadRepository _budgetCodeRepo;
//		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
//		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
//		private readonly IDepartmentReadRepository _departmentRepo;
//		private readonly IUserReadRepostiory _userRepo;
//		private readonly IMapper _mapper;
//		private readonly ICurrentUserService _currentUser;
//		public GetAvailabelBudgetCodesQueryHandler(
//			IBudgetCodeReadRepository budgetCodeRepo, IMapper mapper, ICurrentUserService currentUser, IBudgetPeriodReadRepository budgetPeriodRepo,
//			IDepartmentReadRepository departmentRepo, IUserReadRepostiory userRepo, IBudgetPlanReadRepository budgetPlanRepo
//		)
//		{
//			_budgetCodeRepo = budgetCodeRepo;
//			_budgetPeriodRepo = budgetPeriodRepo;
//			_budgetPlanRepo = budgetPlanRepo;
//			_mapper = mapper;
//			_currentUser = currentUser;
//			_departmentRepo = departmentRepo;
//			_userRepo = userRepo;
//			_currentUser = currentUser;
//		}

//		public async Task<IReadOnlyList<BudgetCodeLookupDto>> Handle(GetAvailabelBudgetCodesQuery query, CancellationToken cancellationToken)
//		{
//			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");
//			var currentUser = await _userRepo.GetByIdProjectedAsync(userId, cancellationToken)
//				?? throw new NotFoundException("User không hợp lệ");

//			var userDepartment = await _departmentRepo.ExistAsync(q => q.Id == currentUser.DepartmentId, cancellationToken);
//			if (!userDepartment) throw new ForbiddenException("User chưa có phòng ban không thể tạo kế hoạch ngân sách");

//			var today = DateTime.UtcNow.Date;
//			var activePeriod = await _budgetPeriodRepo.ListProjectedAsync(
//				q => q.Where(x =>
//					today >= x.StartDate.Date && today <= x.EndDate.Date
//				).Select(x => x.Id),
//				cancellationToken: cancellationToken
//			);
//			if (!activePeriod.Any())
//				return Array.Empty<BudgetCodeLookupDto>();

//			var activeCode = await _budgetCodeRepo.ListProjectedAsync(
//				q => q.Where(x => x.IsActive && !x.IsDeleted)
//					.Select(x => x.Id),
//				cancellationToken: cancellationToken
//			);

//			// Mã ngân sách đã có kế hoạch ngân sách
//			var plannedCodeIds = await _budgetPlanRepo.ListProjectedAsync(
//				q => q.Where(x => x.IsActive && !x.IsDeleted)
//					.Select(x => x.BudgetCodeId),
//					cancellationToken: cancellationToken
//				);

//			var availableCodeIds = activeCode.Except(plannedCodeIds);
//			if (!availableCodeIds.Any())
//				return Array.Empty<BudgetCodeLookupDto>();

//			return await _budgetCodeRepo.ListProjectedAsync(
//				q => q.Where(x => availableCodeIds.Contains(x.Id))
//					.ProjectTo<BudgetCodeLookupDto>(_mapper.ConfigurationProvider),
//				cancellationToken: cancellationToken
//			);

//		}
//	}
//}
