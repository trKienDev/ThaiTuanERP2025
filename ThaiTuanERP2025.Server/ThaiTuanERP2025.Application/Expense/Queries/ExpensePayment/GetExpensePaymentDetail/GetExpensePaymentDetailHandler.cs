using AutoMapper;
using MediatR;
using System.Text.Json;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Followers.Enums;

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

			// 1) Gom toàn bộ GUID liên quan tới user (cả Default, Approved, Rejected)
			var allIds = workflow.Steps
				.SelectMany(s => (s.ResolvedApproverCandidatesJson is { Length: > 0 }
					? JsonSerializer.Deserialize<string[]>(s.ResolvedApproverCandidatesJson) ?? Array.Empty<string>()
					: Array.Empty<string>()))
				.Select(id => Guid.TryParse(id, out var g) ? g : (Guid?)null)
				.Where(g => g.HasValue).Select(g => g!.Value)
				.Concat(workflow.Steps.Select(s => s.DefaultApproverId).Where(g => g.HasValue).Select(g => g!.Value))
				.Concat(workflow.Steps.Select(s => s.ApprovedBy).Where(g => g.HasValue).Select(g => g!.Value))
				.Concat(workflow.Steps.Select(s => s.RejectedBy).Where(g => g.HasValue).Select(g => g!.Value))
				.Distinct()
				.ToArray();

			// 2) Load users batch
			var users = allIds.Length == 0 
				? new List<Domain.Account.Entities.User>()
				: await _unitOfWork.Users.ListByIdsAsync(allIds, cancellationToken);

			// 3) Map sang dict
			var userDtoDict = users.Select(u => _mapper.Map<UserDto>(u)).ToDictionary(u => u.Id, u => u);

			var detail = _mapper.Map<ApprovalWorkflowInstanceDetailDto>(
				workflow,
				opt => { opt.Items["UserDict"] = userDtoDict; }
			);

			var enrichedSteps = detail.Steps
				.Select(s => s with {
					ApproverCandidates = (s.ResolvedApproverCandidateIds ?? Array.Empty<Guid>())
						.Select(id => userDtoDict.TryGetValue(id, out var dto) ? dto : null)
						.Where(u => u != null)
						.ToArray()!
				}).OrderBy(p => p.Order).ToList();

			detail = detail with { Steps = enrichedSteps };

			// 6 ) Followers
			var followerUserIds = await _unitOfWork.Followers.ListAsync(
				q => q.Where(f => f.SubjectType == SubjectType.ExpensePayment && f.SubjectId == payment.Id).Select(f => f.UserId),
				cancellationToken: cancellationToken
			);
			var followerDtos = Array.Empty<UserDto>();
			if (followerUserIds.Count > 0)
			{
				var uidArr = followerUserIds.ToArray();
				var followerUsers = await _unitOfWork.Users.ListAsync(
					q => q.Where(u => uidArr.Contains(u.Id)),
					cancellationToken: cancellationToken
				);
				followerDtos = followerUsers.Select(u => _mapper.Map<UserDto>(u)).ToArray();
			}

			// OutogingPayments
			var outgoingPayments = await _unitOfWork.OutgoingPayments.ListAsync(
				q => q.Where(o => o.ExpensePaymentId == payment.Id)
					.OrderByDescending(o => o.CreatedDate),
				cancellationToken: cancellationToken
			);

			var outgoingDtos = outgoingPayments
				.Select(o => _mapper.Map<OutgoingPaymentStatusDto>(o))
				.ToArray();

			//  Mapping ExpensePaymentDetailDto
			var dto = _mapper.Map<ExpensePaymentDetailDto>(payment) with
			{
				Followers = followerDtos,
				WorkflowInstanceDetail = detail,
				OutgoingPayments = outgoingDtos,
			};

			return dto;
		}
	}
}
