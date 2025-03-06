

namespace Logging
{
    /// <summary>
    /// Concrete class for logging to the console output.
    /// </summary>
    internal class ConsoleLogger : LoggerBase, ILogger
    {
        public ConsoleLogger(Settings settings)
        : base(settings)
        {
        }

        void ILogger.WriteLog(LogLevel level, string subject, string message)
        {
            if(IsSameLevel(level))
            {
                Console.WriteLine(FormatMessage(level, subject, message));
            }
        }
    }
}