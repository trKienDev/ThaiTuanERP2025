using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccounts.GetLedgerAccountsByType
{
	public class GetLedgerAccountsByTypeHandler : IRequestHandler<GetLedgerAccountsByTypeQuery, List<LedgerAccountTreeDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetLedgerAccountsByTypeHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<LedgerAccountTreeDto>> Handle(GetLedgerAccountsByTypeQuery request, CancellationToken cancellationToken) {
			var entities = await _unitOfWork.LedgerAccounts
				.ListAsync(q =>
					q.Where(x => x.LedgerAccountTypeId == request.TypeId)
						.OrderBy(x => x.Number)
				);

			var lookup = entities.ToLookup(x => x.ParrentLedgerAccountId);

			List<LedgerAccountTreeDto> BuildTree(Guid? parentId) {
				return lookup[parentId].Select(x => new LedgerAccountTreeDto
				{
					Id = x.Id,
					Code = x.Number,
					Name = x.Name,
					Description = x.Description,
					IsActive = x.IsActive,
					BalanceType = x.LedgerAccountBalanceType.ToString(),
					Children = BuildTree(x.Id)
				}).ToList();
			}

			return BuildTree(null);
		}
	}
}
