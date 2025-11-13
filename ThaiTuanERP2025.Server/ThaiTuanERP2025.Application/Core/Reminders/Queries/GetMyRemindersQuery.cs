using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Application.Core.Reminders.Queries
{
	public sealed record GetMyRemindersQuery : IRequest<IReadOnlyCollection<UserReminderDto>>;
	public sealed class GetMyRemindersQueryHandler : IRequestHandler<GetMyRemindersQuery, IReadOnlyCollection<UserReminderDto>>
	{
		private readonly IUserReminderReadRepository _reminderRepo;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		public GetMyRemindersQueryHandler(IUserReminderReadRepository reminderRepo, ICurrentUserService currentUserService, IMapper mapper)
		{
			_reminderRepo = reminderRepo;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<IReadOnlyCollection<UserReminderDto>> Handle(GetMyRemindersQuery query, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;

			return await _reminderRepo.ListProjectedAsync (
				q => q.Where(a => a.UserId == uid && !a.IsResolved)
					.OrderByDescending(a => a.CreatedAt)
					.ProjectTo<UserReminderDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			 );
		}
	}
}
