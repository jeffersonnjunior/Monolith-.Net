using Domain.Aggregates.AccountAggregate;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class CheckingAccountRepository : ICheckingAccountRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<CheckingAccount> _accounts;

    public CheckingAccountRepository(AppDbContext context)
    {
        _context = context;
        _accounts = context.Set<CheckingAccount>();
    }

    public async Task<CheckingAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _accounts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(CheckingAccount account, CancellationToken cancellationToken = default)
    {
        await _accounts.AddAsync(account, cancellationToken);
    }

    public Task UpdateAsync(CheckingAccount account, CancellationToken cancellationToken = default)
    {
        _accounts.Update(account);
        return Task.CompletedTask;
    }
}
