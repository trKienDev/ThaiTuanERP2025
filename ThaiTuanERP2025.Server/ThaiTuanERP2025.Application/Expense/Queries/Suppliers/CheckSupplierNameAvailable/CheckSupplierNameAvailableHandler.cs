using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Expense.Queries.Suppliers.CheckSupplierNameAvailable
{
	public class CheckSupplierNameAvailableHandler : IRequestHandler<CheckSupplierNameAvailableQuery, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CheckSupplierNameAvailableHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(CheckSupplierNameAvailableQuery request, CancellationToken cancellationToken) {
			var name = request.name?.Trim() ?? string.Empty;
			if(string.IsNullOrEmpty(name)) 
				return false;

			var exists = await _unitOfWork.Suppliers.ExistsByNameAsync(name, request.ExcludeId, cancellationToken);
			return !exists;
		} 
	}
}
