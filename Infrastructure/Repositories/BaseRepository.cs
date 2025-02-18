using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Utilities.Db;
using Infrastructure.Utilities.FunctionsDatabase;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
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
            _dbSet.Update(obj);
            _unitOfWork.Commit();
        }

        public void Delete(TEntity obj)
        {
            _unitOfWork.BeginTransaction();
            _dbSet.Remove(obj);
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

        public IQueryable<TEntity> GetFilters(Dictionary<string, string> filters, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.ApplyDynamicFilters(filters);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
