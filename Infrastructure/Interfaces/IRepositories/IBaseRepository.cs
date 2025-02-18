using System.Linq.Expressions;

namespace Infrastructure.Interfaces.IRepositories;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
{
    void Add(TEntity obj);
    void Update(TEntity obj);
    void Delete(TEntity obj);
    IQueryable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression, params string[] includes);
    TEntity GetElementByExpression(Expression<Func<TEntity, bool>> expression, params string[] includes);
    IQueryable<TEntity> GetFilters(Dictionary<string, string> filters, params string[] includes);
}