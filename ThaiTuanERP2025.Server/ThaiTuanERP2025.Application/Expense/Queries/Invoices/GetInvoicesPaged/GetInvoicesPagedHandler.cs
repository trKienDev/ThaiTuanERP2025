using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Invoices.GetInvoicesPaged
{
	public sealed class GetInvoicesPagedHandler : IRequestHandler<GetInvoicesPagedQuery, PagedResult<InvoiceDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetInvoicesPagedHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public Task<PagedResult<InvoiceDto>> Handle(GetInvoicesPagedQuery query, CancellationToken cancellationToken)
		{
			return _unitOfWork.Invoices.GetInvoicesPagedAsync(query.Page, query.PageSize, query.Keyword, cancellationToken);
		}
	}
}
