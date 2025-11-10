using AutoMapper;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Users
{
	public class UserMappingProfile : Profile
	{
		public UserMappingProfile() {
			CreateMap<User, UserDto>()
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email != null ? src.Email.Value : null))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone != null ? src.Phone.Value : null))
				.ForMember(dest => dest.AvatarFileId, opt => opt.MapFrom(src => src.AvatarFileId))
				.ForMember(d => d.Roles, o => o.Ignore())
				.ForMember(d => d.Managers, o => o.Ignore())
				.AfterMap<UserAvatarObjectKeyResolver<UserDto>>();

			CreateMap<User, UserBriefDto>();

			CreateMap<User, UserBriefAvatarDto>()
				.AfterMap<UserAvatarObjectKeyResolver<UserBriefAvatarDto>>();

			CreateMap<UserDto, UserBriefAvatarDto>()
				.ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
				.ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
				.ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
				.ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.EmployeeCode))
				.ForMember(d => d.AvatarFileId, o => o.MapFrom(s => s.AvatarFileId))
				.ForMember(d => d.AvatarFileObjectKey, o => o.MapFrom(s => s.AvatarFileObjectKey));
		}

		/// <summary>
		/// Sau khi map xong User → UserDto, resolver này gắn AvatarFileObjectKey cho DTO.
		/// Có hỗ trợ cache và preload dictionary từ context.Items để tối ưu hiệu năng.
		/// </summary>
		public sealed class UserAvatarObjectKeyResolver<TDestination> : IMappingAction<User, TDestination> where TDestination : class, IHasAvatarFile
		{
			private readonly IUnitOfWork _unitOfWork;
			private static readonly Dictionary<Guid, string?> _fileCache = new();

			public UserAvatarObjectKeyResolver(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public void Process(User source, TDestination destination, ResolutionContext context)
			{
				if (source.AvatarFileId is null)
					return;

				var fileId = source.AvatarFileId.Value;

				// preload dictionary (nếu handler đã load sẵn)
				if (context.TryGetItems(out var items) &&
					items.TryGetValue("AvatarDict", out var dictObj) &&
					dictObj is Dictionary<Guid, string> dict &&
					dict.TryGetValue(fileId, out var preloadedKey))
				{
					destination.AvatarFileObjectKey = preloadedKey;
					return;
				}

				// cache nội bộ
				if (_fileCache.TryGetValue(fileId, out var cachedKey))
				{
					destination.AvatarFileObjectKey = cachedKey;
					return;
				}

				// fallback database
				var file = _unitOfWork.StoredFiles.GetByIdAsync(fileId).GetAwaiter().GetResult();
				var objectKey = file?.ObjectKey ?? string.Empty;

				destination.AvatarFileObjectKey = objectKey;

				if (!_fileCache.ContainsKey(fileId))
					_fileCache[fileId] = objectKey;
			}
		}
	}
}
