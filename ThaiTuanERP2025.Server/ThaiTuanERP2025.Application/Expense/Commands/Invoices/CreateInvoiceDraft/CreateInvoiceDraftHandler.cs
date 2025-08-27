using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.CreateInvoiceDraft
{
	public sealed class CreateInvoiceDraftHandler : IRequestHandler<CreateInvoiceDraftCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateInvoiceDraftHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<InvoiceDto> Handle(CreateInvoiceDraftCommand request, CancellationToken cancellationToken)
		{
			var invoice = new Invoice
			{
				Id = Guid.NewGuid(),
				InvoiceName = request.Request.InvoiceName,
				InvoiceNumber = request.Request.InvoiceNumber,
				IssueDate = request.Request.IssueDate,
				PaymentDate = request.Request.PaymentDate,
				SellerName = request.Request.SellerName,
				SellerTaxCode = request.Request.SellerTaxCode,
				SellerAddress = request.Request.SellerAddress,
				BuyerName = request.Request.BuyerName,
				BuyerTaxCode = request.Request.BuyerTaxCode,
				BuyerAddress = request.Request.BuyerAddress,
				IsDraft = true,
			};

			await _unitOfWork.Invoices.AddAsync(invoice);

			// Attach main file if provided
			var mainLink = new InvoiceFile
			{
				Id = Guid.NewGuid(),
				InvoiceId = invoice.Id,
				FileId = request.Request.MainFileId,
				IsMain = true
			};
			await _unitOfWork.InvoiceFiles.AddAsync(mainLink);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// Projection to DTO
			var dto = await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(invoice.Id, cancellationToken)
				?? throw new NotFoundException("không tìm thấy invocie đã tạo");

			return dto;
		}
	}
}
