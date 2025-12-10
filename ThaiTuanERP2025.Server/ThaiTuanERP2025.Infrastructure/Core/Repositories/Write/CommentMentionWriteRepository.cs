using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Write
{
	public sealed class CommentMentionWriteRepository : BaseWriteRepository<CommentMention>, ICommentMentionWriteRepository
	{
		public CommentMentionWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig) { }
	}
}
