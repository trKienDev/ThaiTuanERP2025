using AutoMapper;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Dtos;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Notifications.Entities;

namespace ThaiTuanERP2025.Infrastructure.Notifications.Services
{
	public sealed class TaskReminderService : ITaskReminderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRealtimeNotifier _realtime;
		private readonly IMapper _mapper;
		public TaskReminderService(IUnitOfWork unitOfWork, IRealtimeNotifier realtime, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_realtime = realtime;
			_mapper = mapper;
		}

		public async Task CreateForStepActivationAsync(Guid stepInstanceId, Guid workflowInstanceId, IEnumerable<Guid> userIds, string title, string message, Guid documentId, string documentType, DateTime dueAt, CancellationToken cancellationToken)
		{
			var reminders = userIds.Select(uid => TaskReminder.Create(uid, workflowInstanceId, stepInstanceId, title, message, documentId, documentType, dueAt)).ToList();

			await _unitOfWork.TaskReminders.AddRangeAsync(reminders, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// push realtime
			var dtos = _mapper.Map<List<TaskReminderDto>>(reminders);
			await _realtime.PushRemindersAsync(userIds, dtos, cancellationToken); // event 'ReceiveAlarm'
		}

		public async Task ResolveByStepAsync(Guid stepInstanceId, string reason, CancellationToken cancellationToken)
		{
			var items = await _unitOfWork.TaskReminders.ListAsync(q => 
				q.Where(a => a.StepInstanceId == stepInstanceId && !a.IsResolved), 
				cancellationToken: cancellationToken
			);
			foreach (var it in items) it.Resolve(reason);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// push resolved event (optional)
			var groupedByUser = items.GroupBy(x => x.UserId);
			foreach (var g in groupedByUser)
			{
				await _realtime.PushRemindersResolvedAsync(new[] { g.Key }, g.Select(x => x.Id).ToList(), cancellationToken); // 'ResolveAlarm'
			}
		}

		public async Task ResolveOneAsync(Guid reminderId, string reason, CancellationToken cancellationToken)
		{
			var it = await _unitOfWork.TaskReminders.SingleOrDefaultIncludingAsync(a => a.Id == reminderId, cancellationToken: cancellationToken);
			if (it == null || it.IsResolved) return;
			it.Resolve(reason);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await _realtime.PushRemindersResolvedAsync(new[] { it.UserId }, new List<Guid> { it.Id }, cancellationToken);
		}

		// TaskReminderService.cs (ý tưởng)
		public async Task ResolveByWorkflowAsync(Guid workflowInstanceId, string reason, CancellationToken cancellationToken)
		{
			var reminders = await _unitOfWork.TaskReminders.ListAsync(
				q => q.Where(a => a.WorkflowInstanceId == workflowInstanceId && !a.IsResolved),
				cancellationToken: cancellationToken,
				asNoTracking: false
			);

			if (!reminders.Any()) return;

			reminders.ForEach(a => a.Resolve(reason));
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var userIds = reminders.Select(a => a.UserId).Distinct().ToList();
			var reminderIds = reminders.Select(a => a.Id).ToList();
			await _realtime.PushRemindersResolvedAsync(userIds, reminderIds, cancellationToken);
		}
	}
}
