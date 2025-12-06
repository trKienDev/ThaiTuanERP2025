using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Core.Comments;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public sealed class CommentReadRepository : BaseReadRepository<Comment, CommentDto>, ICommentReadRepository
	{
		public CommentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

		public async Task<CommentDetailDto> GetDetailById(Guid id, CancellationToken cancellationToken)
		{
			var commentDetail = await _dbSet.Include(x => x.CreatedByUser).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
			return _mapper.Map<CommentDetailDto>(commentDetail);
		}

		public async Task<IReadOnlyList<CommentDetailDto>> GetComments(DocumentType documentType, Guid documentId, CancellationToken cancellationToken)
		{
			var comments = await _dbSet
				.Include(x => x.CreatedByUser)
				.Where(x => x.DocumentType == documentType && x.DocumentId == documentId)
				.OrderByDescending(x => x.CreatedAt) // mới → cũ
				.ToListAsync(cancellationToken);

			return _mapper.Map<IReadOnlyList<CommentDetailDto>>(comments);
		} 
	}
}
