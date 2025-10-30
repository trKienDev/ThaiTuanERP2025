using MediatR;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetBudgetCodesWithAmountForPeriod
{
	public class GetBudgetCodesWithAmountForPeriodHandler : IRequestHandler<GetBudgetCodesWithAmountForPeriodQuery, List<BudgetCodeWithAmountDto>>
	{
		private readonly IBudgetCodeReadRepository _budgetCodeReadRepository;
		private readonly ICurrentUserService _currentUserService;
		private readonly IDepartmentReadRepository _departmentReadRepository;
		private readonly IUserReadRepostiory _userReadRepository;
		public GetBudgetCodesWithAmountForPeriodHandler(
			IBudgetCodeReadRepository budgetCodeReadRepository, ICurrentUserService currentUserService,
			IDepartmentReadRepository departmentReadRepository, IUserReadRepostiory userReadRepository
		) {
			_budgetCodeReadRepository = budgetCodeReadRepository;
			_currentUserService = currentUserService;
			_departmentReadRepository = departmentReadRepository;
			_userReadRepository = userReadRepository;
		}

		public async Task<List<BudgetCodeWithAmountDto>> Handle(GetBudgetCodesWithAmountForPeriodQuery request, CancellationToken cancellationToken)
		{
			// 1 ) Lấy user hiện tại
			var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

			// 2) Lấy DepartmentId từ user
			var me = await _userReadRepository.GetByIdAsync(userId, cancellationToken)
				?? throw new NotFoundException("User không tồn tại");

			if (me.DepartmentId.HasValue)
			{
				var exists = await _departmentReadRepository.ExistsAsync( d => d.Id == me.DepartmentId.Value, cancellationToken );
				if (!exists) throw new NotFoundException("Không tìm thấy phòng ban của user nầy");
			} else throw new NotFoundException("User chưa được gán phòng ban");

			// Lấy mặc định theo giờ VN nếu không truyền
			var timezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Asia/Ho_Chi_Minh
			var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timezone);
			var year = request.Year ?? now.Year;
			var month = request.Month ?? now.Month;

			return await _budgetCodeReadRepository.GetWithAmountForPeriodAsync(year, month, me.DepartmentId.Value, cancellationToken);
		}
	}
}
