using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    internal class SessionsMap : IEntityTypeConfiguration<Sessions>
    {
        public void Configure(EntityTypeBuilder<Sessions> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SessionTime)
                .IsRequired();

            builder.Property(s => s.FilmId)
                .IsRequired();

            builder.Property(s => s.FilmAudioOption)
                .IsRequired();

            builder.Property(s => s.FilmFormat)
                .IsRequired();

            builder.HasOne(s => s.Film)
                .WithMany(f => f.Sessions)
                .HasForeignKey(s => s.FilmId);

            builder.HasMany(s => s.Tickets)
                .WithOne(t => t.Session)
                .HasForeignKey(t => t.SessionId);
        }
    }
}