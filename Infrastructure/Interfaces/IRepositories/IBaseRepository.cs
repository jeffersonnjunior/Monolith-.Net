namespace Infrastructure.Interfaces.IRepositories;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
{
    void Add(TEntity obj);
    void Update(TEntity obj);
    void Delete(TEntity obj);
}