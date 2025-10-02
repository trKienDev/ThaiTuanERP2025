using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment
{
	public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public AddDepartmentCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(AddDepartmentCommand command, CancellationToken cancellationToken)
		{
			Department? parentDepartment = null;
			if (command.ParentId.HasValue)
				parentDepartment = await _unitOfWork.Departments.GetByIdAsync(command.ParentId.Value)
				    ?? throw new Exception("Parent department not found");

			// Dùng ctor public: (name, code, region, managerUserId)
			var entity = new Department(command.Name, command.Code, command.Region, command.ManagerId)
			{
				ParentId = command.ParentId
				// IsActive mặc định true trong entity, không cần set lại
			};

			await _unitOfWork.Departments.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
