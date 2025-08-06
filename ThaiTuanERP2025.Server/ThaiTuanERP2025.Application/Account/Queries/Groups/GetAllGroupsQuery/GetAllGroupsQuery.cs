using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.Groups.GetAllGroupsQuery
{
	public record GetAllGroupsQuery() : IRequest<List<GroupDto>>;
}
