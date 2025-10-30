using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Group : AuditableEntity
	{
		private readonly List<UserGroup> _userGroups = new();
		private static readonly Regex SlugRegex = new(@"^[a-z0-9._-]+$", RegexOptions.Compiled);

		private Group() { } // EF only

		public Group(string name, string slug, string description)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(slug, nameof(slug));

			Id = Guid.NewGuid();
			Name = name.Trim();
			Slug = NormalizeSlug(slug ?? Slugify(Name));
			Description = description?.Trim() ?? string.Empty;
			IsActive = true;

			AddDomainEvent(new GroupCreatedEvent(this));
		}

		public string Name { get; private set; } = string.Empty;
		public string Slug { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;
		public Guid AdminId { get; private set; }

		public IReadOnlyCollection<UserGroup> UserGroups => _userGroups.AsReadOnly();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new GroupRenamedEvent(this));
		}

		public void UpdateDescription(string newDescription)
		{
			Description = newDescription?.Trim() ?? string.Empty;
			AddDomainEvent(new GroupDescriptionUpdatedEvent(this));
		}

		public void SetAdmin(Guid userId)
		{
			if (userId == Guid.Empty)
				throw new ArgumentException("UserId không hợp lệ", nameof(userId));

			if (AdminId == userId)
				return;

			AdminId = userId;
			AddDomainEvent(new GroupAdminChangedEvent(this, userId));
		}

		public void SetSlug(string newSlug)
		{
			var normalized = NormalizeSlug(newSlug);
			if (normalized == Slug)
				return;

			Slug = normalized;
			AddDomainEvent(new GroupSlugChangedEvent(this));
		}

		public void AddUser(Guid userId)
		{
			if (_userGroups.Any(ug => ug.UserId == userId))
				return;

			_userGroups.Add(new UserGroup(userId, Id));
			AddDomainEvent(new UserAddedToGroupEvent(this, userId));
		}

		public void RemoveUser(Guid userId)
		{
			var existing = _userGroups.FirstOrDefault(ug => ug.UserId == userId);
			if (existing == null)
				return;

			_userGroups.Remove(existing);
			AddDomainEvent(new UserRemovedFromGroupEvent(this, userId));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new GroupActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new GroupDeactivatedEvent(this));
		}
		#endregion

		#region Helpers
		private static string Slugify(string input)
		{
			var s = input.Trim().ToLowerInvariant();

			// remove diacritics
			var norm = s.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();
			foreach (var c in norm)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					sb.Append(c);
			}

			s = sb.ToString().Normalize(NormalizationForm.FormC);
			s = Regex.Replace(s, @"[^a-z0-9]+", "-");
			s = Regex.Replace(s, @"-+", "-").Trim('-');
			return string.IsNullOrWhiteSpace(s) ? "group" : s;
		}

		private static string NormalizeSlug(string slug)
		{
			if (string.IsNullOrWhiteSpace(slug))
				throw new ArgumentException("Slug không hợp lệ", nameof(slug));

			var h = slug.Trim().ToLowerInvariant();
			if (!SlugRegex.IsMatch(h))
				throw new ArgumentException("Slug chỉ gồm a–z, 0–9, '.', '_', '-'", nameof(slug));

			return h;
		}
		#endregion
	}
}
