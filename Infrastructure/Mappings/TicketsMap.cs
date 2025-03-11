using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class TicketsMap : IEntityTypeConfiguration<Tickets>
{
    public void Configure(EntityTypeBuilder<Tickets> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.SessionId)
            .IsRequired();

        builder.Property(t => t.SeatId)
            .IsRequired();

        builder.Property(t => t.CustomerDetailsId)
            .IsRequired();

        builder.HasOne(t => t.Session)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.SessionId);

        builder.HasOne(t => t.Seat)
            .WithMany(seat => seat.Tickets)
            .HasForeignKey(t => t.SeatId);

        builder.HasOne(t => t.CustomerDetails)
            .WithMany(cd => cd.Tickets)
            .HasForeignKey(t => t.CustomerDetailsId);
    }
}