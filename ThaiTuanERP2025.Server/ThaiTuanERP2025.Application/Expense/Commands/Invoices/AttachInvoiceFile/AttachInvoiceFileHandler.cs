using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AttachInvoiceFile
{
	public sealed class AttachInvoiceFileHandler : IRequestHandler<AttachInvoiceFileCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public AttachInvoiceFileHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<InvoiceDto> Handle(AttachInvoiceFileCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var invoice = await _unitOfWork.Invoices.GetByIdAsync(request.InvoiceId) 
				?? throw new DirectoryNotFoundException("Không tìm thấy hóa đơn");

			// File bổ sung luôn IsMain = false
			var link = new InvoiceFile
			{
				Id = Guid.NewGuid(),
				InvoiceId = request.InvoiceId,
				FileId = request.FileId,
				IsMain = false
			};

			await _unitOfWork.InvoiceFiles.AddAsync(link);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(invoice.Id, cancellationToken)
				?? throw new DirectoryNotFoundException("Không tìm thấy hóa đơn");
		}
	}
}
