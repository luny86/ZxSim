
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
            if(_logger == null)
            {
                // TODO determine type...
                _logger = new ConsoleLogger(_settings);
            }

            return _logger; 
        }
    }
}