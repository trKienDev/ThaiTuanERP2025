using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class ApprovalWorkflowRepository : BaseRepository<ApprovalWorkflow>, IApprovalWorkflowRepository  
	{
		private readonly IConfigurationProvider _mapperCfg;
		private readonly IMapper _mapper;
		public ApprovalWorkflowRepository(
			ThaiTuanERP2025DbContext context, 
			IConfigurationProvider mapperCfg,
			IMapper mapper) : base(context, mapperCfg)
		{
			_mapperCfg = mapperCfg ?? throw new ArgumentNullException(nameof(mapperCfg));
			_mapper = mapper;
		}

		public async Task<ApprovalWorkflow?> SingleOrDefaultIncludingAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.Include(x => x.Steps).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);	
		}

		public async Task<ApprovalWorkflowDto> AddAndReturnDtoAsync(ApprovalWorkflow entity, CancellationToken cancellationToken = default) {
			await _dbSet.AddAsync(entity, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			var materialized = await _dbSet.AsNoTracking()
				.Include(x => x.Steps)
				.SingleAsync(x => x.Id == entity.Id, cancellationToken);

			return _mapper.Map<ApprovalWorkflowDto>(materialized);
		}
	}
}
