using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Notifications.Entities;

namespace ThaiTuanERP2025.Infrastructure.Notifications.Configurations
{
	public class AppNotificationConfiguration : IEntityTypeConfiguration<AppNotification>
	{
		public void Configure(EntityTypeBuilder<AppNotification> builder) {
			builder.ToTable("AppNotifications", "Core");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Title).HasMaxLength(256);
			builder.Property(x => x.Link).HasMaxLength(512);
			builder.Property(x => x.DocumentType).HasMaxLength(64);

			builder.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedDate });
			// Chống tạo trùng thông báo cùng document/step cho 1 user (tuỳ nhu cầu):
			builder.HasIndex(x => new { x.UserId, x.DocumentType, x.DocumentId, x.WorkflowStepInstanceId }).IsUnique(false);
		}
	}
}
