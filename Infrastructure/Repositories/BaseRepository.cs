using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IUnitOfWork _unitOfWork;

    public BaseRepository(DbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _unitOfWork = unitOfWork;
    }

    public void Add(TEntity obj)
    {
        _unitOfWork.BeginTransaction();

        _dbSet.Add(obj);

        _unitOfWork.Commit();
    }
    public void Update(TEntity obj)
    {
        _unitOfWork.BeginTransaction();

        _dbSet.Add(obj);

        _unitOfWork.Commit();
    }
    public void Remove(TEntity obj)
    {
        _unitOfWork.BeginTransaction();

        _dbSet.Add(obj);

        _unitOfWork.Commit();
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}