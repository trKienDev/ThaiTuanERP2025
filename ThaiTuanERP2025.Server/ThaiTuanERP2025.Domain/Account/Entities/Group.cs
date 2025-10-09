using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Group : AuditableEntity
	{
		public Guid AdminId { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public string Slug { get; private set; } = string.Empty;
		private static readonly Regex SlugRegex = new(@"^[a-z0-9._-]+$", RegexOptions.Compiled);
		public string Description { get; private set; } = string.Empty;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public ICollection<UserGroup> UserGroups { get; private set; }

		private Group()
		{
			UserGroups = new List<UserGroup>();
		} // EF
		public Group(string name, string slug, string description)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Tên group  không được để trống");
			Id = Guid.NewGuid();
			Name = name.Trim();
			Slug = NormalizeSlug(slug ?? Slugify(Name));
			Description = description?.Trim() ?? string.Empty;
			UserGroups = new List<UserGroup>();
		}

		public void Rename(string newName)
		{
			if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentNullException("Tên mới không hợp lệ");
			Name = newName.Trim();
		}

		public void UpdateDescription(string newDescription)
		{
			Description = newDescription?.Trim() ?? string.Empty;
		}

		public void SetAdmin(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException("UserId không hợp lệ");
			}

			AdminId = userId;
		}

		public void SetSlug(string newSlug)
		{
			Slug = NormalizeSlug(newSlug);
		}

		private static string Slugify(string input)
		{
			var s = input.Trim().ToLowerInvariant();

			// remove diacritics
			var norm = s.Normalize(NormalizationForm.FormD);
			var sb = new System.Text.StringBuilder();
			foreach (var c in norm)
			{
				var uc = CharUnicodeInfo.GetUnicodeCategory(c);
				if (uc != UnicodeCategory.NonSpacingMark) sb.Append(c);
			}
			s = sb.ToString().Normalize(NormalizationForm.FormC);

			// non [a-z0-9] -> '-'
			s = Regex.Replace(s, @"[^a-z0-9]+", "-");
			// collapse '-'
			s = Regex.Replace(s, @"-+", "-").Trim('-');
			if (string.IsNullOrWhiteSpace(s)) s = "group";
			return s;
		}

		private static string NormalizeSlug(string slug)
		{
			if (string.IsNullOrWhiteSpace(slug)) throw new ArgumentException("Slug không hợp lệ");
			var h = slug.Trim().ToLowerInvariant();
			if (!SlugRegex.IsMatch(h)) throw new ArgumentException("Slug chỉ gồm a–z, 0–9, '.', '_', '-'");
			return h;
		}
	}
}
