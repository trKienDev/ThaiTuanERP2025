using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Group
	{
		public Guid Id { get; private set; }
		public Guid AdminId { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;

		public ICollection<UserGroup> UserGroups { get; private set; }

		private Group()
		{
			UserGroups = new List<UserGroup>();
		} // EF
		public Group(string name, string description)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Tên group  không được để trống");
			Id = Guid.NewGuid();
			Name = name.Trim();
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
	}
}
