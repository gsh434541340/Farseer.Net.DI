namespace FS.DI.Tests.Infrastructure
{
    public interface IUserService : IDependency
    {
        UserEntity GetById(int id);
    }
}
