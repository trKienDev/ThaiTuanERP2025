using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Alerts.TaskReminders.Queries.GetMyReminders
{
	public class GetMyRemindersQueryHandler : IRequestHandler<GetMyRemindersQuery, IReadOnlyCollection<TaskReminderDto>>
	{
		private readonly ITaskReminderReadRepository _taskReminderReadRepo;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;	
		public GetMyRemindersQueryHandler(
			IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper, ITaskReminderReadRepository taskReminderReadRepo
		) {
			_taskReminderReadRepo = taskReminderReadRepo;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<IReadOnlyCollection<TaskReminderDto>> Handle(GetMyRemindersQuery request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;
			var items = await _taskReminderReadRepo.ListProjectedAsync(
				q => q.Where(a => a.UserId == uid && !a.IsResolved)
					.OrderByDescending(a => a.CreatedDate)
					.ProjectTo<TaskReminderDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			 );
			return items;
		}
	}
}
