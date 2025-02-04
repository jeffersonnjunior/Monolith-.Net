namespace Infrastructure.Interfaces;

public interface IUnitOfWork: IDisposable
{
    int SaveChanges();
    void Rollback();
    void BeginTransaction();
    void Commit();
}
