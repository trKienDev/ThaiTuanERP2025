using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts;
using ThaiTuanERP2025.Application.Finance.CashoutGroups;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;

namespace ThaiTuanERP2025.Application.Finance.CashoutCodes.Queries
{
        public sealed record GetCashoutCodeWithGroupTreeQuery : IRequest<IReadOnlyList<CashoutGroupTreeWithCodesDto>>;

        public sealed class GetCashoutCodeWithGroupTreeQueryHandler : IRequestHandler<GetCashoutCodeWithGroupTreeQuery, IReadOnlyList<CashoutGroupTreeWithCodesDto>>
        {
                private readonly ICashoutGroupReadRepository _cashoutGroupRepo;
                private readonly ICashoutCodeReadRepository _cashoutCodeRepo;
                private readonly IMapper _mapper;
                public GetCashoutCodeWithGroupTreeQueryHandler(
                        ICashoutGroupReadRepository cashoutGroupRepo, ICashoutCodeReadRepository cashoutCodeRepo, IMapper mapper
                ) {
                        _cashoutGroupRepo = cashoutGroupRepo;
                        _cashoutCodeRepo = cashoutCodeRepo;     
                        _mapper = mapper;
                }

                public async Task<IReadOnlyList<CashoutGroupTreeWithCodesDto>> Handle(GetCashoutCodeWithGroupTreeQuery query, CancellationToken cancellationToken)
                {
                        var groupList = (await _cashoutGroupRepo.ListProjectedAsync(
                                q => q.Where(x => x.IsActive)
                                        .OrderBy(x => x.Path)
                                        .ProjectTo<CashoutGroupTreeWithCodesDto>(_mapper.ConfigurationProvider),
                                cancellationToken: cancellationToken
                        ));
			// group dictionary
			var groups = groupList.ToDictionary(x => x.Id);

			var codeDtos = await _cashoutCodeRepo.ListProjectedAsync(
                                q => q.Where(x => x.IsActive)
                                        .ProjectTo<CashoutCodeTreeDto>(_mapper.ConfigurationProvider),
                                cancellationToken: cancellationToken
                        );

			// Gán code vào Group theo CashoutGroupDto
			foreach (var code in codeDtos)
			{
				if (groups.TryGetValue(code.CashoutGroupId, out var group))
				{
					group.Codes.Add(code);
				}
			}

			// Build tree bằng ParentId → Children
			// Tạo lookup cho ParentId → Groups con
			var lookup = groups.Values.ToLookup(g => g.ParentId);

			// Hàm dựng tree đệ quy
			List<CashoutGroupTreeWithCodesDto> BuildTree(Guid? parentId)
			{
				return lookup[parentId]
					.OrderBy(x => x.OrderNumber)
					.Select(g => {
					        g.Children = BuildTree(g.Id);
					        return g;
				        }).ToList();
			}

			var roots = BuildTree(null);

			return roots;
                }
        }
}
