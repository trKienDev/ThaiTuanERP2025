using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class UserGroup
	{
		public Guid UserId { get; private set; }
		public User User { get; private set; }
		public Guid GroupId { get; private set; }
		public Group Group { get; private set; }
		public DateTime JoinedAt { get; private set; } = DateTime.UtcNow;

		private UserGroup() {
			User = null!;
			Group = null!;
		}

		public UserGroup(Guid userId, Guid groupId) {
			if(userId == Guid.Empty) throw new ArgumentNullException("UserId không hợp lệ");
			if (groupId == Guid.Empty) throw new ArgumentNullException("GroupId không hợp lệ");

			UserId = userId;
			GroupId = groupId;

			User = null!;
			Group = null!;	
		}
	}
}
