using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllBudgetCodes
{
	public class GetAllBudgetCodesQueryHandler : IRequestHandler<GetAllBudgetCodesQuery, List<BudgetCodeDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllBudgetCodesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<BudgetCodeDto>> Handle(GetAllBudgetCodesQuery request, CancellationToken cancellationToken)
		{
			return await _unitOfWork.BudgetCodes.ListProjectedAsync(
				query => query.Select(x => new BudgetCodeDto {
					Id = x.Id,
					Code = x.Code,
					Name = x.Name,
					BudgetGroupId = x.BudgetGroupId,
					BudgetGroupName = x.BudgetGroup.Name,
					IsActive = x.IsActive
				}),
				true,
				cancellationToken: cancellationToken
			);
		}
	}
}
