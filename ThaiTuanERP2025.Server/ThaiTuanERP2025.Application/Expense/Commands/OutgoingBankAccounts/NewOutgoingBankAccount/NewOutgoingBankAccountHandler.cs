using MediatR;
using System.Text.RegularExpressions;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingBankAccounts.NewOutgoingBankAccount
{
	public sealed class NewOutgoingBankAccountHandler : IRequestHandler<NewOutgoingBankAccountCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public NewOutgoingBankAccountHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(NewOutgoingBankAccountCommand command, CancellationToken cancellationToken) {
			var request = command.Request;

			var name = NormalizeText(request.Name);
			var bankName = NormalizeText(request.BankName);
			var accountNumber = NormalizeAccountNumber(request.AccountNumber);
			var ownerName = NormalizeText(request.OwnerName);

			var existingPair = await _unitOfWork.OutgoingBankAccounts.SingleOrDefaultIncludingAsync(
				b => b.Name == name && b.BankName == bankName && b.AccountNumber == accountNumber,
				cancellationToken: cancellationToken
			);
			if(existingPair is not null) {
				if (string.Equals(existingPair.OwnerName, ownerName, StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException("Outgoing bank account is duplicated (Name, BankName, AccountNumber, OwnerName).");
				}
				throw new InvalidOperationException("Outgoing bank account must be unique by (Name, AccountNumber, BankName).");
			} 

			var entity = new OutgoingBankAccount(name, bankName, accountNumber, ownerName);
			await _unitOfWork.OutgoingBankAccounts.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		private static string NormalizeText(string? s) => (s ?? string.Empty).Trim();

		private static string NormalizeAccountNumber(string? s)
		{
			// Bỏ khoảng trắng, dấu gạch… để so sánh/unique nhất quán
			var raw = (s ?? string.Empty).Trim();
			return Regex.Replace(raw, @"[\s\-]", "");
		}
	}
}
