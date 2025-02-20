using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Utilities.Db;
using Infrastructure.Utilities.FiltersModel;
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

        public TEntity Add(TEntity obj)
        {
            _unitOfWork.BeginTransaction();
            _dbSet.Add(obj);
            _unitOfWork.Commit();
            return obj;
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

        public TEntity GetElementByParameter(FilterByItem filterByItem)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.Property(parameter, filterByItem.Field);
            var constant = Expression.Constant(filterByItem.Value);
            Expression body;

            if (filterByItem.Key == "Equal") body = Expression.Equal(member, constant);

            else body = Expression.NotEqual(member, constant);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            return GetElementByExpression(lambda, filterByItem.Includes);
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