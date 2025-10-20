using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.LedgerAccounts.GetAllLedgerAccounts
{
	public class GetAllLedgerAccountsHandler : IRequestHandler<GetAllLedgerAccountsQuery, List<LedgerAccountDto>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllLedgerAccountsHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<LedgerAccountDto>> Handle(GetAllLedgerAccountsQuery request, CancellationToken cancellationToken) {
			var list = await _unitOfWork.LedgerAccounts.FindIncludingAsync(
				_ => true,
				cancellationToken: cancellationToken,
				asNoTracking: true,
				a => a.LedgerAccountType
			);
			var ordered = list.OrderBy(x => x.Path).ToList();	
			return _mapper.Map<List<LedgerAccountDto>>(ordered);
		}
	}
}
