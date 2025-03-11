using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class CustomerDetailsMap : IEntityTypeConfiguration<CustomerDetails>
{
    public void Configure(EntityTypeBuilder<CustomerDetails> builder)
    {
        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cd => cd.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cd => cd.Age)
            .IsRequired();

        builder.HasMany(cd => cd.Tickets)
            .WithOne(t => t.CustomerDetails)
            .HasForeignKey(t => t.CustomerDetailsId);
    }
}