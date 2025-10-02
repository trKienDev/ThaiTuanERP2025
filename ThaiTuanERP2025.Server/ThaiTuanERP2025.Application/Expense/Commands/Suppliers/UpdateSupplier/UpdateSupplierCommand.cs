using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.Suppliers.UpdateSupplier
{
	public record UpdateSupplierCommand(Guid Id, UpdateSupplierRequest Request) : IRequest<SupplierDto>;
}
