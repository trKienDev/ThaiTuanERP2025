using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetExpensePaymentDetail
{
	public sealed class GetExpensePaymentDetailHandler : IRequestHandler<GetExpensePaymentDetailQuery, ExpensePaymentDetailDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public GetExpensePaymentDetailHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<ExpensePaymentDetailDto> Handle(GetExpensePaymentDetailQuery query, CancellationToken cancellationToken) {
			var payment = await _unitOfWork.ExpensePayments.GetDetailByIdAsync(query.Id, cancellationToken);
			if (payment == null)
				throw new NotFoundException($"ExpensePayment {query.Id} not found.");

			var workflow = await _unitOfWork.ExpensePayments.GetWorkflowInstanceAsync(payment.Id, cancellationToken);
			if (workflow == null)
			{
				var baseDto = _mapper.Map<ExpensePaymentDetailDto>(payment);
				return baseDto;
			}

			// 1) Map phần lõi + steps detail
			var detail = new ApprovalWorkflowInstanceDetailDto
			{
				WorkflowInstance = _mapper.Map<ApprovalWorkflowInstanceDto>(workflow), // map cái lõi
				Steps = _mapper.Map<List<ApprovalStepInstanceDetailDto>>(workflow.Steps) // map detail steps
			};

			// 2) Gom tất cả GUID approver từ mọi step (lọc null & trùng)
			var allIds = detail.Steps
				.SelectMany(s => s.ResolvedApproverCandidateIds ?? Array.Empty<Guid>())
				.Distinct()
				.ToArray();

			// 3) Lấy users theo batch
			var users = allIds.Length == 0
				? new List<Domain.Account.Entities.User>()
				: await _unitOfWork.Users.ListByIdsAsync(allIds, cancellationToken);

			// 4) Map sang UserDto, đưa vào dict để tra nhanh
			var userDtos = _mapper.Map<List<UserDto>>(users);
			var userDict = userDtos.ToDictionary(u => u.Id, u => u);

			// 5) Gán ApproverCandidates cho từng step (record dùng with)
			var enrichedSteps = detail.Steps
				.Select(s => s with
				{
					ApproverCandidates = (s.ResolvedApproverCandidateIds ?? Array.Empty<Guid>())
					.Select(id => userDict.TryGetValue(id, out var dto) ? dto : null)
					.Where(u => u != null)
					.ToArray()!
				})
				.ToList();

			detail = detail with { Steps = enrichedSteps };

			// 6) Nhét vào ExpensePaymentDetailDto
			var dto = _mapper.Map<ExpensePaymentDetailDto>(payment) with
			{
				WorkflowInstanceDetail = detail
			};

			return dto;
		}
	}
}
