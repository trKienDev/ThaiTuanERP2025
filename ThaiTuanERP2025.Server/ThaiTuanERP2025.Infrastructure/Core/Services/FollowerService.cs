using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Services
{
	public sealed class FollowerService : IFollowerService
	{
		private readonly IUnitOfWork _unitOfWork;
		public FollowerService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task FollowAsync(Guid documentId, DocumentType documentType, Guid userId, CancellationToken cancellationToken) {
			bool exists = documentType switch
			{
				DocumentType.ExpensePayment => await _unitOfWork.ExpensePayments.ExistAsync(e => e.Id == documentId, cancellationToken),
				DocumentType.OutgoingPayment => await _unitOfWork.OutgoingPayments.ExistAsync(e => e.Id == documentId, cancellationToken),
				_ => false
			};

			if (!exists)
				throw new NotFoundException($"{documentType}({documentId}) not found");

			// Kiểm tra đã follow chưa
			bool existsFollow = await _unitOfWork.Followers.ExistAsync(
				f => f.DocumentType == documentType && f.DocumentId == documentId && f.UserId == userId,
				cancellationToken
			);

			if (existsFollow)
				return;

			var follower = new Follower(documentId, documentType, userId);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		public async Task FollowManyAsync(DocumentType subjectType, Guid subjectId, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
		{
			var set = new HashSet<Guid>(userIds.Where(u => u != Guid.Empty));
			if (set.Count == 0) return;

			// Lấy những follower đã tồn tại để tránh trùng
			var existing = await _unitOfWork.Followers.ListAsync(
				q => q.Where(f => f.DocumentType == subjectType && f.DocumentId == subjectId && set.Contains(f.UserId)),
				cancellationToken: cancellationToken
			);
			var existedUserIds = existing.Select(f => f.UserId).ToHashSet();

			var toAdd = set.Except(existedUserIds).Select(uid => new Follower(subjectId, subjectType, uid)).ToList();

			if (toAdd.Count > 0)
			{
				await _unitOfWork.Followers.AddRangeAsync(toAdd, cancellationToken);
				await _unitOfWork.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task UnfollowAsync(DocumentType documentType, Guid documentId, Guid userId, CancellationToken cancellationToken) {
			var item = await _unitOfWork.Followers.SingleOrDefaultIncludingAsync(
				f => f.DocumentType == documentType && 
					f.DocumentId == documentId &&
					f.UserId == userId,
				cancellationToken: cancellationToken
			);

			if (item is null) return;

			_unitOfWork.Followers.Delete(item);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}

		public async Task<bool> IsFollowingAsync(DocumentType documentType, Guid documentId, Guid userId, CancellationToken cancellationToken) {
			return await _unitOfWork.Followers.ExistAsync(
				f => f.DocumentType == documentType && f.DocumentId == documentId && f.UserId == userId,
				cancellationToken
			);
		}
	}
}
