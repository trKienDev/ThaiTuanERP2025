using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Queries.Suppliers.CheckSupplierNameAvailable
{
	public record CheckSupplierNameAvailableQuery(string name, Guid? ExcludeId = null) : IRequest<bool>;
}
