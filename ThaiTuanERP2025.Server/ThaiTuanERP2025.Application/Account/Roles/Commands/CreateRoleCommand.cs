using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Account.Roles.Request;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands
{
	public sealed record CreateRoleCommand(RoleRequest Request) : IRequest<Unit>;

	public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateRoleCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var exists = await _unitOfWork.Roles.SingleOrDefaultIncludingAsync(r => r.Name == request.Name, cancellationToken: cancellationToken);
			if (exists is not null)
				throw new InvalidOperationException($"Role '{request.Name}' đã tồn tại.");

			var role = new Role(request.Name, request.Description);

			await _unitOfWork.Roles.AddAsync(role, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}

	public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
	{
		public CreateRoleCommandValidator()
		{
			RuleFor(x => x.Request.Name)
				.NotEmpty().WithMessage("Role name is required.")
				.MaximumLength(100);

			RuleFor(x => x.Request.Description).MaximumLength(255).WithMessage("Mô tả không vượt quá 255 ký tự");
		}
	}
}
