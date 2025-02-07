using Infrastructure.Interfaces;
using Infrastructure.Utilities.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public IQueryable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression, params string[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return query.Where(expression);
    }

    public TEntity GetElementByExpression(Expression<Func<TEntity, bool>> expression, params string[] includes)
    {
        return _dbSet.Includes(includes).AsNoTracking().FirstOrDefault(expression);
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}