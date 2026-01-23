using Domain.ValueObjects;

namespace Domain.Aggregates.LoanAggregate;

public class Installment
{
    public Guid Id { get; private set; }
    public int Number { get; private set; }
    public decimal Value { get; private set; }
    public InstallmentStatus Status { get; private set; }
    public DateTime? PaidDate { get; private set; }

    protected Installment() { }

    public Installment(int number, decimal value)
    {
        Id = Guid.NewGuid();
        Number = number;
        Value = value;
        Status = InstallmentStatus.Pending;
    }

    internal void MarkAsPaid()
    {
        Status = InstallmentStatus.Paid;
        PaidDate = DateTime.UtcNow;
    }
}