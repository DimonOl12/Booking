using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message> {
	public void Configure(EntityTypeBuilder<Message> builder) {
		builder.ToTable("Messages");

		builder.Property(m => m.Text)
			.HasMaxLength(4000)
			.IsRequired();

		builder.Property(m => m.UpdatedAtUtc)
			.IsRequired(false);

		builder.HasOne(m => m.Author)
			.WithMany(u => u.Messages)
			.HasForeignKey(m => m.AuthorId)
			.IsRequired();
	}
}
