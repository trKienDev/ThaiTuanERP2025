using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
	{
		public void Configure(EntityTypeBuilder<UserNotification> builder) {
			builder.ToTable("UserNotifications", "Core");

			builder.HasKey(n => n.Id);

			builder.Property(n => n.Title).IsRequired().HasMaxLength(256);

			builder.Property(n => n.Message).IsRequired();

			builder.Property(n => n.LinkUrl).HasMaxLength(500);

			builder.Property(n => n.Type)
				.HasConversion<int>()
				.IsRequired();

			builder.Property(n => n.IsRead).IsRequired();

			builder.Property(n => n.ReadAt).HasColumnType("datetime2");

			builder.HasOne(n => n.Sender)
			       .WithMany()
			       .HasForeignKey(n => n.SenderId)
			       .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(n => n.Receiver)
				.WithMany()
				.HasForeignKey(n => n.ReceiverId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
