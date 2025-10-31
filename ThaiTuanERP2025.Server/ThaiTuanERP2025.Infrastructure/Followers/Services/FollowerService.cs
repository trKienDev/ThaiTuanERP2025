using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Followers.Services;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Followers.Entities;
using ThaiTuanERP2025.Domain.Followers.Enums;
using ThaiTuanERP2025.Domain.Followers.ValueObjects;

namespace ThaiTuanERP2025.Infrastructure.Followers.Services
{
	public sealed class FollowerService : IFollowerService
	{
		private readonly IUnitOfWork _unitOfWork;
		public FollowerService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task FollowAsync(SubjectType subjectType, Guid subjectId, Guid userId, CancellationToken cancellationToken) {
			bool exists = subjectType switch
			{
				SubjectType.ExpensePayment => await _unitOfWork.ExpensePayments.AnyAsync(e => e.Id == subjectId, cancellationToken),
				SubjectType.OutgoingPayment => await _unitOfWork.OutgoingPayments.AnyAsync(e => e.Id == subjectId, cancellationToken),
				SubjectType.Invoice => await _unitOfWork.Invoices.AnyAsync(e => e.Id == subjectId, cancellationToken),
				_ => false
			};

			if (!exists)
				throw new NotFoundException($"{subjectType}({subjectId}) not found");

			// Kiểm tra đã follow chưa
			bool existsFollow = await _unitOfWork.Followers.AnyAsync(
				f => f.Subject.Type == subjectType && f.Subject.Id == subjectId && f.UserId == userId,
				cancellationToken
			);

			if (existsFollow)
				return;

			var follower = new Follower(
				new SubjectRef (subjectType, subjectId),
				userId
			);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		public async Task FollowManyAsync(SubjectType subjectType, Guid subjectId, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
		{
			var set = new HashSet<Guid>(userIds.Where(u => u != Guid.Empty));
			if (set.Count == 0) return;

			// Lấy những follower đã tồn tại để tránh trùng
			var existing = await _unitOfWork.Followers.ListAsync(
				q => q.Where(f => f.Subject.Type == subjectType && f.Subject.Id == subjectId && set.Contains(f.UserId)),
				cancellationToken: cancellationToken
			);
			var existedUserIds = existing.Select(f => f.UserId).ToHashSet();

			var toAdd = set.Except(existedUserIds).Select(uid => new Follower(new SubjectRef(subjectType, subjectId), uid)).ToList();

			if (toAdd.Count > 0)
			{
				await _unitOfWork.Followers.AddRangeAsync(toAdd, cancellationToken);
				await _unitOfWork.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task UnfollowAsync(SubjectType subjectType, Guid subjectId, Guid userId, CancellationToken cancellationToken) {
			var item = await _unitOfWork.Followers.SingleOrDefaultIncludingAsync(
				f => f.Subject.Type == subjectType && 
					f.Subject.Id == subjectId &&
					f.UserId == userId,
				cancellationToken: cancellationToken
			);

			if (item is null) return;

			_unitOfWork.Followers.Delete(item);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		public async Task<bool> IsFollowingAsync(SubjectType subjectType, Guid subjectId, Guid userId, CancellationToken cancellationToken) {
			return await _unitOfWork.Followers.AnyAsync(
				f => f.Subject.Type == subjectType && f.Subject.Id == subjectId && f.UserId == userId,
				cancellationToken
			);
		}
	}
}
