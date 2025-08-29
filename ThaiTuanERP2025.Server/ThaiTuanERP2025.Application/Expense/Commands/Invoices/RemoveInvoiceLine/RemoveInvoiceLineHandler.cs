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

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceLine
{
	public sealed class RemoveInvoiceLineHandler : IRequestHandler<RemoveInvoiceLineCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public RemoveInvoiceLineHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<InvoiceDto> Handle(RemoveInvoiceLineCommand command, CancellationToken cancellationToken) {
			var invoice = await _unitOfWork.Invoices.GetByIdAsync(command.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");
			var line = await _unitOfWork.InvoiceLines.GetByIdAsync(command.LineId)
				?? throw new NotFoundException("Không tìm thấy dòng hóa đơn");
			if(line.InvoiceId != invoice.Id)
				throw new ConflictException("Dòng hóa đơn không thuộc về hóa đơn đã chọn");

			_unitOfWork.InvoiceLines.Delete(line);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(command.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");
		}
	}
}
