using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Followers.Entities;

namespace ThaiTuanERP2025.Infrastructure.Followers.Configurations
{
	public sealed class FollowerConfiguration : IEntityTypeConfiguration<Follower>
	{
		public void Configure(EntityTypeBuilder<Follower> builder)
		{
			builder.ToTable("Followers", "Core");

			builder.HasKey(f => f.Id);

			builder.Property(f => f.UserId).IsRequired();
			builder.Property(f => f.IsActive).IsRequired();

			builder.HasQueryFilter(f => !f.IsDeleted);

			builder.ComplexProperty(f => f.Subject, subject =>
			{
				subject.Property(s => s.Type)
				    .HasColumnName("SubjectType")
				    .HasConversion<int>()
				    .IsRequired();

				subject.Property(s => s.Id)
				    .HasColumnName("SubjectId")
				    .IsRequired();
			});

			builder.HasOne<User>()
			       .WithMany()
			       .HasForeignKey(f => f.UserId)
			       .OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex("SubjectType", "SubjectId", "UserId")
			       .IsUnique();
		}
	}
}
