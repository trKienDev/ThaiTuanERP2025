using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Mappings
{
	public class DivisionMappingProfile : Profile {
		public DivisionMappingProfile() {
			CreateMap<Division, DivisionDto>()
				.ForMember(d => d.HeadUserName, m => m.MapFrom(s => s.HeadUser != null ? s.HeadUser.FullName : null))
				.ForMember(d => d.DepartmentCount, m => m.MapFrom(s => s.Departments.Count));

			CreateMap<Division, DivisionSummaryDto>()
				.ForMember(d => d.HeadUserName, m => m.MapFrom(s => s.HeadUser != null ? s.HeadUser.FullName : null))
				.ForMember(d => d.DepartmentCount, m => m.MapFrom(s => s.Departments.Count));

			// Request ==> Domain
			CreateMap<CreateDivisionRequest, Division>()
				.ConstructUsing(s => new Division(
					s.Name.Trim(),
					(s.Description == null ? string.Empty : s.Description.Trim()),
					s.HeadUserId
				));

			CreateMap<UpdateDivisionRequest, Division>()
				.ForAllMembers(_ => { });
		}
	}
}
