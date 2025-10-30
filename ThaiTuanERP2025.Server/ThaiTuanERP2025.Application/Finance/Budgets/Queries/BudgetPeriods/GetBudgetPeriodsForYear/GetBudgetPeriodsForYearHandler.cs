using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Queries.BudgetPeriods.GetBudgetPeriodsForYear
{
	public sealed class GetBudgetPeriodsForYearHandler : IRequestHandler<GetBudgetPeriodsForYearQuery, IReadOnlyList<BudgetPeriodDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetBudgetPeriodsForYearHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<IReadOnlyList<BudgetPeriodDto>> Handle(GetBudgetPeriodsForYearQuery query, CancellationToken cancellationToken) {
			var year = query.Year;

			var periods = await _unitOfWork.BudgetPeriods.ListAsync(
				q => q.Where(x => x.Year == query.Year).OrderBy(x => x.Month),
				cancellationToken: cancellationToken
			);

			return _mapper.Map<IReadOnlyList<BudgetPeriodDto>>(periods);
		}
	}
}
