using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.CashOutCodes.GetAllCashOutCodes
{
	public class GetAllCashOutCodesHandler : IRequestHandler<GetAllCashOutCodesQuery, List<CashOutCodeDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllCashOutCodesHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<CashOutCodeDto>> Handle(GetAllCashOutCodesQuery request, CancellationToken cancellationToken) {
			var list = await _unitOfWork.CashOutCodes.FindIncludingAsync(
				_ => true,
				x => x.CashOutGroup,
				x => x.PostingLedgerAccount
			);
			var ordered = list.OrderBy(x => x.Code).ToList();
			return _mapper.Map<List<CashOutCodeDto>>(ordered);
		}
	}
}
