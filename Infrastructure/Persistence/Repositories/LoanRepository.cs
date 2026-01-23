using Domain.Aggregates.LoanAggregate;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Loan> _loans;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
        _loans = context.Set<Loan>();
    }

    public async Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _loans
            .Include(x => x.Installments)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        await _loans.AddAsync(loan, cancellationToken);
    }

    public Task UpdateAsync(Loan loan, CancellationToken cancellationToken = default)
    {
        _loans.Update(loan);
        return Task.CompletedTask;
    }
}
