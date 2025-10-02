using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Commands.Group.CreateGroup;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.CreateGroup
{
	public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CreateGroupCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
		{
			var group = new Domain.Account.Entities.Group(request.Name, request.Slug, request.Description);
			group.SetAdmin(request.AdminUserId);

			var userGroup = new UserGroup(request.AdminUserId, group.Id);
			await _unitOfWork.Groups.AddAsync(group);
			await _unitOfWork.UserGroups.AddAsync(userGroup);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<GroupDto>(group);
		}
	}
}
