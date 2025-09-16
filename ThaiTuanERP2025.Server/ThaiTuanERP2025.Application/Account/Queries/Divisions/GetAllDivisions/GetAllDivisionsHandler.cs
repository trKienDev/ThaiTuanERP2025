using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Account.Queries.Divisions.GetAllDivisions
{
	public class GetAllDivisionsHandler : IRequestHandler<GetAllDivisionsQuery, List<DivisionSummaryDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetAllDivisionsHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<List<DivisionSummaryDto>> Handle(GetAllDivisionsQuery query, CancellationToken cancellationToken)
		{
			var divisions = await _unitOfWork.Divisions.GetAllAsync();
			return divisions.Select(d => new DivisionSummaryDto
			{
				Id = d.Id,
				Name = d.Name,
			}).ToList();
		}
	}
}
