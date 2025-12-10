using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public sealed class LedgerAccountReadRepository : BaseReadRepository<LedgerAccount, LedgerAccountDto>, ILedgerAccountReadRepository
	{
		public LedgerAccountReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

                public async Task<IReadOnlyList<LedgerAccountDto>> GetByTypeAsync(Guid typeId, CancellationToken cancellationToken = default)
                {
                        return await _dbSet.Where(x => x.LedgerAccountTypeId == typeId && x.IsActive)
                                .OrderBy(x => x.Path)
                                .Select(x => new LedgerAccountDto {
                                            Id = x.Id,
                                            Number = x.Number,
                                            Name = x.Name,
                                            LedgerAccountTypeId = x.LedgerAccountTypeId,
                                            ParentLedgerAccountId = x.ParentLedgerAccountId,
                                            Description = x.Description,
                                            Level = x.Level,
                                            Path = x.Path,
                                            BalanceType = x.LedgerAccountBalanceType,
                                            IsActive = x.IsActive
                                })
                                .ToListAsync(cancellationToken);
                }

		public async Task<IReadOnlyList<LedgerAccountDto>> GetAllActiveAsync(CancellationToken cancellationToken = default)
		{
			return await _dbSet
				.Where(x => x.IsActive)   
				.OrderBy(x => x.Path)
				.Select(x => new LedgerAccountDto
				{
					Id = x.Id,
					Number = x.Number,
					Name = x.Name,
					LedgerAccountTypeId = x.LedgerAccountTypeId,
					ParentLedgerAccountId = x.ParentLedgerAccountId,
					Description = x.Description,
					Level = x.Level,
					Path = x.Path,
					BalanceType = x.LedgerAccountBalanceType,
					IsActive = x.IsActive
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<LedgerAccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet
				.Where(x => x.Id == id)
				.Select(x => new LedgerAccountDto
				{
					Id = x.Id,
					Number = x.Number,
					Name = x.Name,
					LedgerAccountTypeId = x.LedgerAccountTypeId,
					ParentLedgerAccountId = x.ParentLedgerAccountId,
					Description = x.Description,
					Level = x.Level,
					Path = x.Path,
					BalanceType = x.LedgerAccountBalanceType,
					IsActive = x.IsActive
				})
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IReadOnlyList<LedgerAccountTreeDto>> GetTreeAsync(CancellationToken cancellationToken = default)
		{
			return await _dbSet
				.Where(x => x.IsActive)
				.OrderBy(x => x.Path)
				.Select(x => new LedgerAccountTreeDto
				{
					Id = x.Id,
					ParentId = x.ParentLedgerAccountId,

					Number = x.Number,
					Name = x.Name,
					BalanceType = x.LedgerAccountBalanceType,

					Level = x.Level,
					Path = x.Path,
					Description = x.Description,

					LedgerAccountTypeName = x.LedgerAccountType != null
						? x.LedgerAccountType.Name
						: null
				})
				.ToListAsync(cancellationToken);
		}
	}
}
