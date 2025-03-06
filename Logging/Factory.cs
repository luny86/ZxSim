
namespace Logging
{
    /// <summary>
    /// Main factory for creating loggers.
    /// </summary>
    internal class Factory : IFactory
    {
        private ILogger _logger = null!;

        private Settings _settings;

        public Factory(Settings settings)
        {
            _settings = settings;
        }

        ILogger IFactory.GetLogger()
        {
            // TODO determine type...
            _logger ??= CreateLogger(_settings.Type);

            return _logger; 
        }

        private ILogger CreateLogger(LogType type) =>
            type switch
            {
                LogType.Console => new ConsoleLogger(_settings),
                LogType.TextFile => new TextFileLogger(_settings),
                _ => throw new ArgumentException(nameof(type), "Invalid LogType used when creating logger.")
            };
    }
}