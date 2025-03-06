
namespace Logging
{
    /// <summary>
    /// Current settings describing the behaviour of the logging.
    /// </summary>
    internal class Settings
    {
        public Settings()
        {
            Path = Directory.GetCurrentDirectory();
        }

        public Settings(Settings copy)
        {
            LevelsToReport = copy.LevelsToReport;
            Type = copy.Type;
            Path = copy.Path;
        }

        /// <summary>
        /// Determines which log levels are reported / outputted.
        /// </summary>
        public LogLevel LevelsToReport
        {
            get;
            set;
        } = LogLevel.Warning | LogLevel.Info;

        /// <summary>
        /// Type of logger to use.
        /// </summary>
        public LogType Type
        {
            get;
            set;
        } = LogType.TextFile;

        /// <summary>
        /// Determines where to put any file output.
        /// </summary>
        public string Path
        {
            get;
            set;
        }
    }
}