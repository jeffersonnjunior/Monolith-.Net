namespace Infrastructure.Interfaces.IRepositories;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();
    void Rollback();
    void BeginTransaction();
    void Commit();
}
