using MediatR;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts;

namespace ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Queries
{
	public sealed record GetAllOutgoingBankAccountsQuery : IRequest<IReadOnlyList<OutgoingBankAccountDto>>;
	public sealed class GetAllOutgoingBankAccountsQueryHandler : IRequestHandler<GetAllOutgoingBankAccountsQuery, IReadOnlyList<OutgoingBankAccountDto>>
	{
		private readonly IOutgoingBankAccountReadRepository _outgoingBankRepo;
		public GetAllOutgoingBankAccountsQueryHandler(IOutgoingBankAccountReadRepository outgoingBankRepo)
		{
			_outgoingBankRepo = outgoingBankRepo;
		}

		public async Task<IReadOnlyList<OutgoingBankAccountDto>> Handle(GetAllOutgoingBankAccountsQuery query, CancellationToken cancellationToken)
		{
			return await _outgoingBankRepo.GetAllAsync(cancellationToken: cancellationToken);	
		}
	}
}