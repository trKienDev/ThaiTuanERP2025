using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetMyInvoices
{
	public class GetMyInvoicesHandler : IRequestHandler<GetMyInvoicesQuery, PagedResult<InvoiceDto>>
	{	
		private readonly ICurrentUserService _currentUserService;
		private readonly IUnitOfWork _unitOfWork;
		public GetMyInvoicesHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public Task<PagedResult<InvoiceDto>> Handle(GetMyInvoicesQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUserService.UserId ?? throw new NotFoundException("Không thể xác định người dùng hiện tại");
			return _unitOfWork.Invoices.GetByCreatorPagedAsync(userId, query.Page, query.PageSize, cancellationToken);
		}
	}
}
