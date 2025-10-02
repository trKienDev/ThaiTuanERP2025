using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
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
				IsDraft = false,
			};

			await _unitOfWork.Invoices.AddAsync(invoice);

			// Attach main file if provided
			if(request.Request.MainFileId.HasValue) {
				var mainLink = new InvoiceFile
				{
					Id = Guid.NewGuid(),
					InvoiceId = invoice.Id,
					FileId = request.Request.MainFileId.Value,
					IsMain = true
				};
				await _unitOfWork.InvoiceFiles.AddAsync(mainLink);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// Projection to DTO
			var dto = await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(invoice.Id, cancellationToken)
				?? throw new NotFoundException("không tìm thấy invocie đã tạo");

			return dto;
		}
	}
}
