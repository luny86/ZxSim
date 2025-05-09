
namespace Logging
{
    /// <summary>
    /// Describes the interface for a logger object.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Determines if logger outputs or not.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Writes a simple string message to the logger.
        /// </summary>
        /// <param name="level">Level of log message.</param>
        /// <param name="subject">Module name or scope name.</param>
        /// <param name="message">Message to write.</param>
        void WriteLog(LogLevel level, string subject, string message);
    }
}