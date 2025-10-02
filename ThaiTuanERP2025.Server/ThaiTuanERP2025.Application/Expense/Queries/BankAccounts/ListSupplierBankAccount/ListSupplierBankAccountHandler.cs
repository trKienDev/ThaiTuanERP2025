using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.ListSupplierBankAccount
{
	public sealed class ListSupplierBankAccountHandler : IRequestHandler<ListSupplierBankAccountQuery, IReadOnlyList<BankAccountDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public ListSupplierBankAccountHandler(IUnitOfWork unitOfWork, IMapper mapper) {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<BankAccountDto>> Handle(ListSupplierBankAccountQuery query, CancellationToken cancellationToken) {
			var list = await _unitOfWork.BankAccounts.ListBySupplierIdAsync(query.SupplierId, cancellationToken);
			return list.Select(_mapper.Map<BankAccountDto>).ToList();
		}
	}
}
