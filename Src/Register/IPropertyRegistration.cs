namespace FS.DI.Register
{
    public interface IPropertyRegistration<out TRegistration>
    {
        TRegistration AsPropertyDependency();
    }
}
