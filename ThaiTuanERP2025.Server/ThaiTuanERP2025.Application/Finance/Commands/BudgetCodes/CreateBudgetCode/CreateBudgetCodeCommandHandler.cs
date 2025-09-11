using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode
{
	public class CreateBudgetCodeCommandHandler : IRequestHandler<CreateBudgetCodeCommand, BudgetCodeDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CreateBudgetCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<BudgetCodeDto> Handle(CreateBudgetCodeCommand request, CancellationToken cancellationToken)
		{
			var entity = new BudgetCode
			{
				Id = Guid.NewGuid(),
				Code = request.Code,
				Name = request.Name,
				BudgetGroupId = request.BudgetGroupId,
				CreatedDate = DateTime.UtcNow,
				IsActive = true
			};

			await _unitOfWork.BudgetCodes.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return _mapper.Map<BudgetCodeDto>(entity);
		}
	}
}
