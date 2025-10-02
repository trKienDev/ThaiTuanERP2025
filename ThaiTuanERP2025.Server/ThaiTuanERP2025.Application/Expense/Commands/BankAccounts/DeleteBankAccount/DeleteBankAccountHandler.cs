using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.DeleteBankAccount
{
	public sealed class DeleteBankAccountHandler : IRequestHandler<DeleteBankAccountCommand, bool> 
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteBankAccountHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteBankAccountCommand command, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.BankAccounts.GetByIdAsync(command.Id) 
				?? throw new DirectoryNotFoundException("Không tìm thấy tài khoản ngân hàng được chỉ định");

			_unitOfWork.BankAccounts.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
