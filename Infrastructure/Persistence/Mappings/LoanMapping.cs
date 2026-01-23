using Domain.Aggregates.LoanAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Mappings;

internal sealed class LoanMapping : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasMany(x => x.Installments)
            .WithOne()
            .HasForeignKey("LoanId")
            .OnDelete(DeleteBehavior.Cascade);

        var installmentsNavigation = builder.Metadata.FindNavigation(nameof(Loan.Installments));
        installmentsNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        installmentsNavigation?.SetField("_installments");
    }
}
