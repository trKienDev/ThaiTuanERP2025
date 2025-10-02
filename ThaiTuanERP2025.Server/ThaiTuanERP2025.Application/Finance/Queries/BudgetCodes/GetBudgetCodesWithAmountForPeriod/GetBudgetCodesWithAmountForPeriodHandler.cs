using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetBudgetCodesWithAmountForPeriod
{
	public class GetBudgetCodesWithAmountForPeriodHandler : IRequestHandler<GetBudgetCodesWithAmountForPeriodQuery, List<BudgetCodeWithAmountDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		public GetBudgetCodesWithAmountForPeriodHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;	
		}

		public async Task<List<BudgetCodeWithAmountDto>> Handle(GetBudgetCodesWithAmountForPeriodQuery request, CancellationToken cancellationToken)
		{
			// 1 ) Lấy user hiện tại
			var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

			// 2) Lấy DepartmentId từ user
			var me = await _unitOfWork.Users.GetByIdAsync(userId) ?? throw new NotFoundException("Không tìm thấy user này");                        // :contentReference[oaicite:1]{index=1}
			var departmentId = me.DepartmentId ?? throw new NotFoundException("User này không có phòng ban");

			// Lấy mặc định theo giờ VN nếu không truyền
			var timezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Asia/Ho_Chi_Minh
			var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timezone);
			var year = request.Year ?? now.Year;
			var month = request.Month ?? now.Month;

			return await _unitOfWork.BudgetCodes.GetWithAmountForPeriodAsync(year, month, departmentId, cancellationToken);
		}
	}
}
