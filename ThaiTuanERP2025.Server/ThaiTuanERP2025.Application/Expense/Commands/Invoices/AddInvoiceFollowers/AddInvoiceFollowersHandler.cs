using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceFollowers
{
	public class AddInvoiceFollowersHandler : IRequestHandler<AddInvoiceFollowersCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AddInvoiceFollowersHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<InvoiceDto> Handle(AddInvoiceFollowersCommand command, CancellationToken cancellationToken) {
			var invoice = await _unitOfWork.Invoices.GetByIdAsync(command.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn yêu cầu");
			
			var existingFollower = await _unitOfWork.InvoiceFollowers.SingleOrDefaultIncludingAsync(
				x => x.InvoiceId == command.InvoiceId && x.UserId == command.UserId
			);
			if(existingFollower is null) {
				await _unitOfWork.InvoiceFollowers.AddAsync(new InvoiceFollwer {
					InvoiceId = command.InvoiceId,
					UserId = command.UserId	
				});
				await _unitOfWork.SaveChangesAsync(cancellationToken);
			}
			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto> (command.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn yêu cầu");
		} 
	}
}
