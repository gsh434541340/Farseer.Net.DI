namespace FS.DI.Tests.Infrastructure
{
    public interface IRepository<TEntity> : IDependency
    {
        TEntity GetById(int id);
    }
}
