using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.CashoutCodes.GetAllCashoutCodes
{
	public class GetAllCashoutCodesHandler : IRequestHandler<GetAllCashoutCodesQuery, List<CashoutCodeDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllCashoutCodesHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<CashoutCodeDto>> Handle(GetAllCashoutCodesQuery request, CancellationToken cancellationToken) {
			var list = await _unitOfWork.CashoutCodes.FindIncludingAsync(
				_ => true,
				cancellationToken: cancellationToken,
				asNoTracking: true,
				x => x.CashoutGroup,
				x => x.PostingLedgerAccount
			);
			var ordered = list.OrderBy(x => x.Code).ToList();
			return _mapper.Map<List<CashoutCodeDto>>(ordered);
		}
	}
}
