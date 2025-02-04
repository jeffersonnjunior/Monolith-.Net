using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _context.SaveChanges();
            _transaction.Commit();
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public void Rollback()
    {
        _transaction.Rollback();
        DisposeTransaction();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        DisposeTransaction();
        _context.Dispose();
    }

    private void DisposeTransaction()
    {
        _transaction.Dispose();
        _transaction = null;

    }
}