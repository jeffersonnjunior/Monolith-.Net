using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class MovieTheatersMap : IEntityTypeConfiguration<MovieTheaters>
{
    public void Configure(EntityTypeBuilder<MovieTheaters> builder)
    {
        builder.HasKey(mt => mt.Id);

        builder.Property(mt => mt.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(mt => mt.TheaterLocationId)
            .IsRequired();

        builder.HasOne(mt => mt.TheaterLocation)
            .WithOne()
            .HasForeignKey<MovieTheaters>(mt => mt.TheaterLocationId);

        builder.HasMany(mt => mt.Screens)
            .WithOne(s => s.MovieTheater)
            .HasForeignKey(s => s.MovieTheaterId);
    }
}