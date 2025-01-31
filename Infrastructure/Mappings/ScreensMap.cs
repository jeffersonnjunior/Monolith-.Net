using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class ScreensMap : IEntityTypeConfiguration<Screens>
{
    public void Configure(EntityTypeBuilder<Screens> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ScreenNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(s => s.SeatingCapacity)
            .IsRequired();

        builder.Property(s => s.MovieTheaterId)
            .IsRequired();

        builder.HasOne(s => s.MovieTheater)
            .WithMany(mt => mt.Screens)
            .HasForeignKey(s => s.MovieTheaterId);

        builder.HasMany(s => s.Seats)
            .WithOne(seat => seat.Screen)
            .HasForeignKey(seat => seat.ScreenId);
    }
}