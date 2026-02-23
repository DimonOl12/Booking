using EasyStay.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations.Identity;

internal class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer> {
	public void Configure(EntityTypeBuilder<Customer> builder) {
		builder.ToTable("Customers");

		builder.Property(c => c.DateOfBirth)
			.IsRequired(false);

		builder.Property(c => c.Address)
			.HasMaxLength(255)
			.IsRequired(false);
	}
}
