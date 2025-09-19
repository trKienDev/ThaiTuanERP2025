using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetUserManagers
{
	public sealed class GetUserManagersHandler : IRequestHandler<GetUserManagersQuery, List<UserDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetUserManagersHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<UserDto>> Handle(GetUserManagersQuery request, CancellationToken cancellationToken)
		{
			var managers = await _unitOfWork.Users.GetManagersAsync(request.UserId, cancellationToken);
			return _mapper.Map<List<UserDto>>(managers);
		}
	}
}
