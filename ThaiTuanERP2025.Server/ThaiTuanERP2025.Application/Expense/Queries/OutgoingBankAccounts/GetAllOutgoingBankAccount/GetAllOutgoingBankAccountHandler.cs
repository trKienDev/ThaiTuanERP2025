using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingBankAccounts.GetAllOutgoingBankAccount
{
	public sealed class GetAllOutgoingBankAccountHandler : IRequestHandler<GetAllOutgoingBankAccountQuery, IReadOnlyList<OutgoingBankAccountDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetAllOutgoingBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<OutgoingBankAccountDto>> Handle(GetAllOutgoingBankAccountQuery query, CancellationToken cancellationToken) {
			var entities = await _unitOfWork.OutgoingBankAccounts.ListAsync(
				q => q.Where(b => b.IsActive == true),
				cancellationToken: cancellationToken
			);
			return _mapper.Map<IReadOnlyList<OutgoingBankAccountDto>>(entities);
		}
	}
}
