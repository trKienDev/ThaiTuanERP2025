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
				.ForMember(d => d.Department, o => o.Ignore())
				.ForMember(d => d.Roles, o => o.Ignore())
				.ForMember(d => d.Managers, o => o.Ignore())
				.AfterMap<UserAvatarObjectKeyResolver>();

			CreateMap<User, UserBriefDto>();
		}

		/// <summary>
		/// Sau khi map xong User → UserDto, resolver này gắn AvatarFileObjectKey cho DTO.
		/// Có hỗ trợ cache và preload dictionary từ context.Items để tối ưu hiệu năng.
		/// </summary>
		public sealed class UserAvatarObjectKeyResolver : IMappingAction<User, UserDto>
		{
			private readonly IUnitOfWork _unitOfWork;
			private static readonly Dictionary<Guid, string?> _fileCache = new();

			public UserAvatarObjectKeyResolver(IUnitOfWork unitOfWork)
			{
				_unitOfWork = unitOfWork;
			}

			public void Process(User source, UserDto destination, ResolutionContext context)
			{
				// Không có avatar → bỏ qua
				if (source.AvatarFileId is null)
					return;

				var fileId = source.AvatarFileId.Value;

				// 1️. Nếu context có preload dictionary (truyền từ handler)
				if (context.TryGetItems(out var items) &&
					items.TryGetValue("AvatarDict", out var dictObj) &&
					dictObj is Dictionary<Guid, string> dict &&
					dict.TryGetValue(fileId, out var preloadedKey)
				)
				{
					destination.AvatarFileObjectKey = preloadedKey;
					return;
				}

				// 2️. Nếu cache nội bộ có sẵn
				if (_fileCache.TryGetValue(fileId, out var cachedKey))
				{
					destination.AvatarFileObjectKey = cachedKey;
					return;
				}

				// 3️. Truy vấn database (fallback)
				var file = _unitOfWork.StoredFiles.GetByIdAsync(fileId).GetAwaiter().GetResult();
				var objectKey = file?.ObjectKey ?? string.Empty;

				destination.AvatarFileObjectKey = objectKey;

				// 4️. Cache lại cho lần sau
				if (!_fileCache.ContainsKey(fileId))
					_fileCache[fileId] = objectKey;
			}
		}
	}
}
