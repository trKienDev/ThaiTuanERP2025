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

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.RemoveInvoiceFile
{
	public sealed class RemoveInvoiceFileHandler : IRequestHandler<RemoveInvoiceFileCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public RemoveInvoiceFileHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<InvoiceDto> Handle(RemoveInvoiceFileCommand command, CancellationToken cancellationToken) {
			var invocie = await _unitOfWork.Invoices.GetByIdAsync(command.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");
			var link = await _unitOfWork.InvoiceFiles.SingleOrDefaultIncludingAsync(x => x.InvoiceId == command.InvoiceId && x.FileId == command.FileId)
				?? throw new NotFoundException("Không tìm thấy file yêu cầu");

			if (link.IsMain)
				throw new ConflictException("Không thể xóa tài liệu hóa đơn chính");

			_unitOfWork.InvoiceFiles.Delete(link);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(command.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");
		}
	}
}
