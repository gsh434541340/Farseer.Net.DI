namespace FS.DI.Tests.Infrastructure
{
    public class Logger : ILogger
    {
        public void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
