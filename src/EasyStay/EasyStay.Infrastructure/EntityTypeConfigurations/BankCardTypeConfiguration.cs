using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class BankCardTypeConfiguration : IEntityTypeConfiguration<BankCard> {
	public void Configure(EntityTypeBuilder<BankCard> builder) {
		builder.ToTable("BankCards");

		builder.Property(bc => bc.Number)
			.HasMaxLength(16)
			.IsRequired();

		builder.Property(bc => bc.ExpirationDate)
			.IsRequired();

		builder.Property(bc => bc.Cvv)
			.HasMaxLength(3)
			.IsRequired();

		builder.Property(bc => bc.OwnerFullName)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(bc => bc.CustomerId)
			.IsRequired(false);
	}
}
