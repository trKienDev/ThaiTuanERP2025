using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Dtos;

namespace ThaiTuanERP2025.Application.Notifications.Queries.TaskReminders.GetMyReminders
{
	public class GetMyRemindersHandler : IRequestHandler<GetMyRemindersQuery, IReadOnlyCollection<TaskReminderDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;	
		public GetMyRemindersHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<IReadOnlyCollection<TaskReminderDto>> Handle(GetMyRemindersQuery request, CancellationToken cancellationToken)
		{
			var uid = _currentUserService.UserId;
			var items = await _unitOfWork.TaskReminders.ListProjectedAsync<TaskReminderDto>(
				q => q.Where(a => a.UserId == uid && !a.IsResolved)
					.OrderBy(a => a.DueAt)
					.ProjectTo<TaskReminderDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			 );
			return items;
		}
	}
}
