namespace MyBank.Core.Aggregates.AccountAggregate;

public class CheckingAccount
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal Balance { get; private set; }

    protected CheckingAccount() { }

    public CheckingAccount(Guid customerId, decimal initialBalance)
    {
        if (initialBalance < 0) throw new DomainException("Saldo inicial inválido.");
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Balance = initialBalance;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
           throw new DomainException("Valor inválido.");


        if (Balance < amount)
            throw new DomainException("Saldo insuficiente.");

        Balance -= amount;
    }
}