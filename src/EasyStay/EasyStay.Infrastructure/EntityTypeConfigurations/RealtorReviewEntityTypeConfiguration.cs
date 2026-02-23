using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class RealtorReviewEntityTypeConfiguration : IEntityTypeConfiguration<RealtorReview> {
	public void Configure(EntityTypeBuilder<RealtorReview> builder) {
		builder.ToTable("RealtorReviews");

		builder.Property(rr => rr.Description)
			.HasMaxLength(4000)
			.IsRequired();

		builder.Property(rr => rr.Score)
			.IsRequired(false);

		builder.Property(rr => rr.UpdatedAtUtc)
			.IsRequired(false);

		builder.HasOne(rr => rr.Author)
			.WithMany(c => c.RealtorReviews)
			.HasForeignKey(r => r.AuthorId)
			.IsRequired();

		builder.HasOne(rr => rr.Realtor)
			.WithMany(r => r.Reviews)
			.HasForeignKey(rr => rr.RealtorId)
			.IsRequired();
	}
}
