using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.DeleteDepartment
{
	public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand>
        {
                private readonly IUnitOfWork _unitOfWork;

                public DeleteDepartmentHandler( IUnitOfWork unitOfWork)
                {
                        _unitOfWork = unitOfWork;
		}

                public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
                {
                        var department = await _unitOfWork.Departments.GetByIdAsync(request.id);
                        if (department == null)
                                throw new NotFoundException("Department not found");

                        _unitOfWork.Departments.Delete(department);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
                }
                }
}
