using ThaiTuanERP2025.Application.Core.Comments;
using ThaiTuanERP2025.Application.Files;
using ThaiTuanERP2025.Application.StoredFiles.Authorization.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Constants;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Application.StoredFiles.Authorization
{
	public sealed class CommentAttachmentPermissionChecker : IStoredFilePermissionChecker
	{
		private readonly ICommentReadRepository _commentRepo;
		public CommentAttachmentPermissionChecker(ICommentReadRepository commentRepo)
		{
			_commentRepo = commentRepo;	
		}

		public bool CanHandle(string module, string entity)
			=> string.Equals(module, ThaiTuanERPModules.Core, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(entity, ExpenseFileEntities.CommentAttachment, StringComparison.OrdinalIgnoreCase);

		public async Task<bool> HasPermissionAsync(StoredFile file, Guid userId, CancellationToken cancellationToken)
		{
			var comment = await _commentRepo.
		}
	}
}
