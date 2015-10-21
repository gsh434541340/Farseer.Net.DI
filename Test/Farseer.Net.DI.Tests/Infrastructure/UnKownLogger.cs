namespace FS.DI.Tests.Infrastructure
{
    public class UnKownLogger : ILogger
    {
        public static ILogger Instance { get; } = new UnKownLogger();
        private UnKownLogger() { }
        public void Debug(string message)
        {
        }
    }
}
