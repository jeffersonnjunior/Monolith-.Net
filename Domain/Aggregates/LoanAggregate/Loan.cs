using Domain.Exceptions;

namespace Domain.Aggregates.LoanAggregate;

public class Loan
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal TotalAmount { get; private set; }

    private readonly List<Installment> _installments = new();
    public IReadOnlyCollection<Installment> Installments => _installments.AsReadOnly();

    protected Loan() { }

    public Loan(Guid customerId, decimal totalAmount, int installmentsCount)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        TotalAmount = totalAmount;
        GenerateInstallments(installmentsCount);
    }

    private void GenerateInstallments(int count)
    {
        var value = TotalAmount / count;
        for (int i = 1; i <= count; i++)
        {
            _installments.Add(new Installment(i, value));
        }
    }

    public void PayInstallment(int installmentNumber)
    {
        var installment = _installments.FirstOrDefault(x => x.Number == installmentNumber);

        if (installment == null)
            throw new DomainException($"Parcela {installmentNumber} não encontrada.");

        if (installment.Status.IsPaid)
        {
            return;
        }

        installment.MarkAsPaid();
    }
}