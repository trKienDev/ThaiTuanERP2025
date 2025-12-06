using AutoMapper;
using ThaiTuanERP2025.Application.Core.Comments;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public sealed class CommentReadRepository : BaseReadRepository<Comment, CommentDto>, ICommentReadRepository
	{
		public CommentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }


	}
}
