
namespace Logging
{
    /// <summary>
    /// Main factory for creating loggers.
    /// </summary>
    internal class Factory : IFactory
    {
        private Dictionary<string, ILogger> _logPool = new Dictionary<string, ILogger>();

        private Settings _settings;

        public Factory(Settings settings)
        {
            _settings = settings;
        }

        ILogger IFactory.GetLogger(string name)
        {
            if (!_logPool.TryGetValue(name, out ILogger? logger))
            {
                logger = CreateLogger(_settings.Type);
                // TODO determine type...
                _logPool.Add(name, logger);
            }

            return logger;
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