using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Comments
{
	public interface ICommentReadRepository : IBaseReadRepository<Comment, CommentDto>
	{
	}
}
