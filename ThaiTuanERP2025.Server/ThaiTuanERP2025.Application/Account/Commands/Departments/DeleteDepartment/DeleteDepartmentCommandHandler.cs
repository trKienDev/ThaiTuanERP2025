using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment
{
    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentRepository _repo;

        public DeleteDepartmentHandler(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _repo.GetByIdAsync(request.id);
            if (department == null)
                throw new Exception("Department not found");

            await _repo.DeleteAsync(request.id);
            return Unit.Value;
        }
    }
}
