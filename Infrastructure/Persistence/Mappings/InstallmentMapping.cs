using Domain.Aggregates.LoanAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Mappings;

internal sealed class InstallmentMapping : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("Installments");

        builder.HasKey(x => x.Id);

        builder.Property<Guid>("LoanId")
            .IsRequired();

        builder.Property(x => x.Number)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        var statusConverter = new ValueConverter<InstallmentStatus, int>(
            status => status.Id,
            id => id switch
            {
                1 => InstallmentStatus.Pending,
                2 => InstallmentStatus.Paid,
                3 => InstallmentStatus.Overdue,
                _ => InstallmentStatus.Pending
            });

        builder.Property(x => x.Status)
            .HasConversion(statusConverter)
            .IsRequired();

        builder.Property(x => x.PaidDate);
    }
}
