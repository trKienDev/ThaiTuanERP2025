using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceFollower
{
	public sealed class RemoveInvoiceFollowerHandler : IRequestHandler<RemoveInvoiceFollowerCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public RemoveInvoiceFollowerHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<InvoiceDto> Handle(RemoveInvoiceFollowerCommand request, CancellationToken cancellationToken)
		{
			var invoice = await _unitOfWork.Invoices.GetByIdAsync(request.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");

			var link = await _unitOfWork.InvoiceFollowers.SingleOrDefaultIncludingAsync(
				x => x.InvoiceId == request.InvoiceId && x.UserId == request.UserId
			) ?? throw new NotFoundException("Không tìm thấy người theo dõi cần xóa");

			_unitOfWork.InvoiceFollowers.Delete(link);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(request.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");

		}
	}
}
