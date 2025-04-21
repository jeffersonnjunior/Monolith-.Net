using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.FiltersModel;
using Infrastructure.FunctionsDatabase;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Db;
using Infrastructure.Notifications;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IUnitOfWork _unitOfWork;
        private readonly NotificationContext _notificationContext;

        public BaseRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _unitOfWork = unitOfWork;
            _notificationContext = notificationContext;
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

        public (TEntity, bool) GetElementEqual(FilterByItem filterByItem)
        {
            if (filterByItem.Includes != null && !ValidadeIncludes(filterByItem.Includes)) return (null, true);

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.Property(parameter, filterByItem.Field);
            var constant = Expression.Constant(filterByItem.Value);
            Expression body;

            if (filterByItem.Key == "Equal") body = Expression.Equal(member, constant);

            else body = Expression.NotEqual(member, constant);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            return (GetElementByExpression(lambda, filterByItem.Includes), false);
        }
        
        public (FilterReturn<TEntity>, bool) GetFilters(Dictionary<string, string> filters, int pageSize, int pageNumber, params string[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes is not null)
            {
                if (ValidadeIncludes(includes)) query = includes.Aggregate(query, (current, include) => current.Include(include));
                
                else return (null, true);
            }

            return (query.ApplyDynamicFilters(filters, pageSize, pageNumber), false);
        }

        public bool ValidadeIncludes(string[] includes)
        {
            foreach (var include in includes)
            {
                var properties = include.Split('.');
                var type = typeof(TEntity);

                foreach (var property in properties)
                {
                    var propertyInfo = type.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo is null)
                    {
                        _notificationContext.AddNotification($"Não é valido esse include: {include}");
                        return false;
                    }
                    type = propertyInfo.PropertyType;
                }
            }
            return true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}