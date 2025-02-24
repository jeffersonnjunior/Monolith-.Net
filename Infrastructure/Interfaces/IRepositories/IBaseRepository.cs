using Infrastructure.Utilities.FiltersModel;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces.IRepositories;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
{
    TEntity Add(TEntity obj);
    void Update(TEntity obj);
    void Delete(TEntity obj);
    IQueryable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression, params string[] includes);
    TEntity GetElementByExpression(Expression<Func<TEntity, bool>> expression, params string[] includes);
    TEntity GetElementByParameter(FilterByItem filterByItem);

    FilterReturn<TEntity> GetFilters(Dictionary<string, string> filters, int pageSize, int pageNumber, params string[] includes);
}