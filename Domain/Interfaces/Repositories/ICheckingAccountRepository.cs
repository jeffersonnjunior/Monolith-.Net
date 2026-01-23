using Domain.Aggregates.AccountAggregate;

namespace Domain.Interfaces.Repositories;

public interface ICheckingAccountRepository
{
    Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(CheckingAccount account, CancellationToken cancellationToken = default);
    Task UpdateAsync(CheckingAccount account, CancellationToken cancellationToken = default);
}
