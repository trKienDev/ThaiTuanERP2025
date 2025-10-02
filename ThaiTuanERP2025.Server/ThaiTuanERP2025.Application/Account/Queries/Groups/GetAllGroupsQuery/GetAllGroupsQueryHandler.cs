using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Account.Queries.Groups.GetAllGroupsQuery
{
	public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, List<GroupDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<List<GroupDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
		{
			var groups = await _unitOfWork.Groups.GetAllAsync();
			return _mapper.Map<List<GroupDto>>(groups);
		}
	}
}
