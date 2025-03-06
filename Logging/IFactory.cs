
namespace Logging
{
    /// <summary>
    /// Describes the factory for creating loggers.
    /// </summary>
    public interface IFactory
    {
        ILogger GetLogger();
    }
}