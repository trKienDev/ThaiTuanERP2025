//using AutoMapper;
//using MediatR;
//using ThaiTuanERP2025.Application.Common.Interfaces;
//using ThaiTuanERP2025.Application.Common.Security;
//using ThaiTuanERP2025.Application.Exceptions;
//using ThaiTuanERP2025.Domain.Common;

//namespace ThaiTuanERP2025.Application.Account.Users.Commands.Create
//{
//	public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
//	{
//		private readonly IUnitOfWork _unitOfWork;
//		private readonly IMapper _mapper;
//		private readonly IPasswordHasher _passwordHasher;
		
//		public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
//		{
//			_unitOfWork = unitOfWork;
//			_mapper = mapper;
//			_passwordHasher = passwordHasher;
//		}

//		public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
//		{
//			var request = command.Request;

//			var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken)
//				?? throw new NotFoundException("Phòng ban không tồn tại");

//			var roles = 

//			var passwordHash = _passwordHasher.HashPassword(request.Password);
//			var user = new Domain.Account.Entities.User(
//				fullName: command.UserRequest.FullName,
//				username: command.UserRequest.Username,
//				employeeCode: command.UserRequest.EmployeeCode,
//				passwordHash: passwordHash,
//				position: command.UserRequest.Position,
//				departmentId: command.UserRequest.DepartmentId,
//				email: command.UserRequest.Email != null ? new Domain.Account.ValueObjects.Email(command.UserRequest.Email) : null,
//				phone: command.UserRequest.Phone != null ? new Domain.Account.ValueObjects.Phone(command.UserRequest.Phone) : null,
//				avatarFileId: command.UserRequest.AvatarFileId
//			);
//			await userRepo.AddAsync(user, cancellationToken);
//			await _unitOfWork.SaveChangesAsync(cancellationToken);
//			return Unit.Value;
//		}
//	}
//}
