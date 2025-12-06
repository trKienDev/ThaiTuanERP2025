using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Comments
{
	public interface ICommentReadRepository : IBaseReadRepository<Comment, CommentDto>
	{
		Task<CommentDetailDto> GetDetailById(Guid id, CancellationToken cancellationToken);
		Task<IReadOnlyList<CommentDetailDto>> GetComments(string module, string entity, Guid entityId, CancellationToken cancellationToken);
	}
}
