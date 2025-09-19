using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;

namespace ThaiTuanERP2025.Application.Account.Queries.Users.GetUserManagerIds
{
	public sealed class GetUserManagerIdsHandler : IRequestHandler<GetUserManagerIdsQuery, List<Guid>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetUserManagerIdsHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<Guid>> Handle(GetUserManagerIdsQuery request, CancellationToken cancellationToken)
		{
			return await _unitOfWork.Users.GetManagerIdsAsync(request.UserId, cancellationToken);
		}
	}
}
