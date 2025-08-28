using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.Invoices.AddInvoiceLine
{
	public sealed class AddInvoiceLineHandler : IRequestHandler<AddInvoiceLineCommand, InvoiceDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AddInvoiceLineHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<InvoiceDto> Handle(AddInvoiceLineCommand command, CancellationToken cancellationToken) {
			var request = command.Request;
			var invoice = await _unitOfWork.Invoices.GetByIdAsync(command.Request.InvoiceId)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");

			var line = new InvoiceLine
			{
				Id = Guid.NewGuid(),
				InvoiceId = request.InvoiceId,
				ItemName = request.ItemName,
				Unit = request.Unit,
				Quantity = request.Quantity,
				UnitPrice = request.UnitPrice,
				DiscountRate = request.DiscountRate,
				DiscountAmount = request.DiscountAmount,
				TaxRatePercent = request.TaxRatePercent,
				WHTTypeId = request.WHTTypeId
			};

			// Simple compute - có thể refactor sang Domain service nếu cần
			var grossAmount = request.Quantity * request.UnitPrice;
			var discountAmount = request.DiscountAmount ?? (request.DiscountRate.HasValue ? Math.Round(grossAmount * request.DiscountRate.Value / 100m, 2) : 0m);
			line.NetAmount = Math.Round(grossAmount - discountAmount, 2);

			line.VATAmount = line.TaxRatePercent.HasValue ? Math.Round(line.NetAmount.Value * line.TaxRatePercent.Value / 100m, 2) : 0m;

			if (request.WHTTypeId.HasValue)
			{
				var whtType = await _unitOfWork.WithholdingTaxTypes.GetByIdAsync(request.WHTTypeId.Value) ?? throw new NotFoundException("Không tìm thấy thuế TNCN đã chọn");
				line.WHTAmount = Math.Round(line.NetAmount.Value * whtType.Rate / 100m, 2);
			}

			line.LineTotal = line.NetAmount.Value + (line.VATAmount ?? 0m) - (line.WHTAmount ?? 0m);

			await _unitOfWork.InvoiceLines.AddAsync(line);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return await _unitOfWork.Invoices.GetByIdProjectedAsync<InvoiceDto>(invoice.Id)
				?? throw new NotFoundException("Không tìm thấy hóa đơn");
		}
	}
}
