using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetOutgoingPaymentDetail
{
	public sealed class GetOutgoingPaymentDetailHandler : IRequestHandler<GetOutgoingPaymentDetailQuery, OutgoingPaymentDetailDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public GetOutgoingPaymentDetailHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<OutgoingPaymentDetailDto> Handle(GetOutgoingPaymentDetailQuery query, CancellationToken cancellationToken)
		{
			var outgoingPayment = await _unitOfWork.OutgoingPayments.GetByIdAsync(query.Id, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy khoản chi được yêu cầu");
			
			var expensePayment = await _unitOfWork.ExpensePayments.GetDetailByIdAsync(outgoingPayment.ExpensePaymentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy thanh toán liên quan đến khoản chi này");
			var expensePaymentDto = _mapper.Map<ExpensePaymentDetailDto>(expensePayment);

			UserDto createdByUser = _mapper.Map<UserDto>(
				await _unitOfWork.Users.GetByIdAsync(outgoingPayment.CreatedByUserId, cancellationToken)
			);

			Supplier? supplier = null;
			if (outgoingPayment.SupplierId.HasValue)
				supplier = await _unitOfWork.Suppliers.GetByIdAsync(outgoingPayment.SupplierId.Value, cancellationToken);
			var supplierDto = _mapper.Map<SupplierDto?>(supplier);

			User? employee = null;
			if (outgoingPayment.EmployeeId.HasValue)
				employee = await _unitOfWork.Users.GetByIdAsync(outgoingPayment.EmployeeId.Value, cancellationToken);	
			var employeeDto = _mapper.Map<UserDto?>(employee);

			var outgoingBankAccount = await _unitOfWork.OutgoingBankAccounts.GetByIdAsync(outgoingPayment.OutgoingBankAccountId, cancellationToken);
			var outgoingBankAccountDto = _mapper.Map<OutgoingBankAccountDto>(outgoingBankAccount);

			var outgoingPaymentDto = _mapper.Map<OutgoingPaymentDetailDto>(outgoingPayment) with
			{
				ExpensePayment = expensePaymentDto,
				Supplier = supplierDto,
				Employee = employeeDto,
				OutgoingBankAccount = outgoingBankAccountDto,
				CreatedByUser = createdByUser
			};
			return outgoingPaymentDto;
		}
	}
}
