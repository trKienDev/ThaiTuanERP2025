using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Expense.GetCommentsByExpensePaymentId
{
	public sealed class GetCommentsByExpensePaymentIdHandler : IRequestHandler<GetCommentsByExpensePaymentIdQuery, IReadOnlyList<ExpensePaymentCommentDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public GetCommentsByExpensePaymentIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<ExpensePaymentCommentDto>> Handle(GetCommentsByExpensePaymentIdQuery query, CancellationToken cancellationToken) {
			// load all comments for the given ExpensePaymentId
			var dtos = await _unitOfWork.ExpensePaymentComments.ListProjectedAsync<ExpensePaymentCommentDto>(
				q => q.Where(c => c.ExpensePaymentId == query.ExpensePaymentId)
					.ProjectTo<ExpensePaymentCommentDto>(_mapper.ConfigurationProvider)
					.OrderBy(d => d.CreatedDate),
				cancellationToken: cancellationToken
			);
			if (dtos.Count == 0)
				return Array.Empty<ExpensePaymentCommentDto>();

			// dictionary for quick lookup
			var dictionary = dtos.ToDictionary(x => x.Id);
			foreach (var dto in dtos)
			{
				if (dto.ParentCommentId is { } pid && dictionary.TryGetValue(pid, out var parent))
				{
					parent.Replies.Add(dto); 
				}
			}

			var topLevel = dtos.Where(x => x.ParentCommentId == null)
							.OrderBy(x => x.CreatedDate)
							.ToList();
			return topLevel;
		}
	}
}
