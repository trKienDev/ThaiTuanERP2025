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
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.ReplaceMainInvoiceFile
{
	public class ReplaceMainInvoiceFileHandler : IRequestHandler<ReplaceMainInvoiceFileCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public ReplaceMainInvoiceFileHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<InvoiceDto> Handle(ReplaceMainInvoiceFileCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var invoice = await _unitOfWork.Invoices.GetByIdAsync(request.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");

			// Bỏ main cũ
			var mains = await _unitOfWork.InvoiceFiles.ListAsync(q => q.Where(x => x.InvoiceId == request.InvoiceId && x.IsMain));
			foreach (var main in mains)
			{
				main.IsMain = false;
			}

			// set main mới
			var link = new InvoiceFile
			{
				Id = Guid.NewGuid(),
				InvoiceId = request.InvoiceId,
				FileId = request.NewFileId,
				IsMain = true
			};
			await _unitOfWork.InvoiceFiles.AddAsync(link);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(request.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");
		}
	}
}
