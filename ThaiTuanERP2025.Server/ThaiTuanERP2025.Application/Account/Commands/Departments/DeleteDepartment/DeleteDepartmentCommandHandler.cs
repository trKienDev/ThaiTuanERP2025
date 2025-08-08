using AutoMapper;
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
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment
{
        public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand>
        {
                private readonly IMapper _mapper;
                private readonly IUnitOfWork _unitOfWork;

                public DeleteDepartmentHandler(IMapper mapper, IUnitOfWork unitOfWork)
                {
                        _unitOfWork = unitOfWork;
			_mapper = mapper;
		}

                public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
                {
                        var department = await _unitOfWork.Departments.GetByIdAsync(request.id);
                        if (department == null)
                                throw new NotFoundException("Department not found");

                        await _unitOfWork.Departments.DeleteAsync(request.id);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
                }
                }
}
