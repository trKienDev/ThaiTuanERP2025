using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts;

namespace ThaiTuanERP2025.Application.Finance.CashoutCodes.Queries
{
	public record GetAllCashoutCodesQuery() : IRequest<IReadOnlyList<CashoutCodeDto>>;
	public class GetAllCashoutCodesHandler : IRequestHandler<GetAllCashoutCodesQuery, IReadOnlyList<CashoutCodeDto>>
	{
		private readonly ICashoutCodeReadRepository _cashoutCodeReadRepo;
		private readonly IMapper _mapper;
		public GetAllCashoutCodesHandler(ICashoutCodeReadRepository cashoutCodeReadRepository, IMapper mapper)
		{
			_cashoutCodeReadRepo = cashoutCodeReadRepository;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<CashoutCodeDto>> Handle(GetAllCashoutCodesQuery query, CancellationToken cancellationToken)
		{
			return await _cashoutCodeReadRepo.ListProjectedAsync(
				q => q.Where(cc => cc.IsActive && !cc.IsDeleted)
					.ProjectTo<CashoutCodeDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}
