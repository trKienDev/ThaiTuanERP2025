using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.UpdateDepartment
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand>
    {
        private readonly IDepartmentRepository _repo;

        public UpdateDepartmentHandler(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dept = await _repo.GetByIdAsync(request.Id)
                       ?? throw new Exception("Department not found");
            
            dept.Name = request.Name;
            await _repo.UpdateAsync(dept);


            return Unit.Value;
        }
    }
}
