namespace Infrastructure.Interfaces;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
{
    void Add(TEntity obj);
    void Update(TEntity obj);
    void Remove(TEntity obj);
}