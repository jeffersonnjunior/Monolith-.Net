using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class TheaterLocationMap : IEntityTypeConfiguration<TheaterLocation>
{
    public void Configure(EntityTypeBuilder<TheaterLocation> builder)
    {
        builder.HasKey(tl => tl.Id);

        builder.Property(tl => tl.Street)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tl => tl.UnitNumber)
            .IsRequired()
            .HasMaxLength(9999);

        builder.Property(tl => tl.PostalCode)
            .IsRequired()
            .HasMaxLength(20);
    }
}