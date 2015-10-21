namespace FS.DI.Core
{
    /// <summary>
    /// 作用域解析器提供者
    /// </summary>
    public interface IScopedResolverProvider
    {
        IScopedResolver CreateScopedResolver();
    }
}
