
using System.Globalization;

namespace Logging
{
    /// <summary>
    /// Base class for a logger.
    /// </summary>
    /// <remarks>
    /// This class supplies some formatting methods for a
    /// consistant text output.
    /// </remarks>
    internal abstract class LoggerBase
    {

        public LoggerBase(Settings settings)
        {
            Settings = new Settings(settings);
        }

        protected Settings Settings
        {
            get;
        }

        protected bool IsSameLevel(LogLevel level)
        {
            return (Settings.LevelsToReport & level) == level;
        }

        /// <summary>
        /// Creates the first part of a logging line
        ///     <date/time>:<level name>:
        /// </summary>
        /// <param name="level">Level output is using.</param>
        /// <returns>Standard dated logging header.</returns>
        static private string CreateHeader(LogLevel level)
        {
            return $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} [{level.ToString()}] ";
        }

        /// <summary>
        /// Format the message to include date, time and log level.
        /// </summary>
        static protected string FormatMessage(LogLevel level, string subject, string message)
        {
            return CreateHeader(level)+$"{subject}:{message}";
        }
    }
}