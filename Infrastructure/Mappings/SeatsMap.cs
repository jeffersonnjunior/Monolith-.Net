using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

internal class SeatsMap : IEntityTypeConfiguration<Seats>
{
    public void Configure(EntityTypeBuilder<Seats> builder)
    {
        builder.HasKey(seat => seat.Id);

        builder.Property(seat => seat.SeatNumber)
            .IsRequired();

        builder.Property(seat => seat.RowLetter)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(seat => seat.ScreenId)
            .IsRequired();

        builder.HasOne(seat => seat.Screen)
            .WithMany(screen => screen.Seats)
            .HasForeignKey(seat => seat.ScreenId);

        builder.HasMany(seat => seat.Tickets)
            .WithOne(ticket => ticket.Seat)
            .HasForeignKey(ticket => ticket.SeatId);
    }
}