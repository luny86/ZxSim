
namespace  Logging
{
    /// <summary>
    /// The different available levels for repotring log message.
    /// </summary>
    /// <remarks>
    /// Each type can be used to set a level when writing a log,
    /// and used as a flag when determine which logs are written.
    [Flags]
    public enum LogLevel : int
    {
        Info = 1,
        Warning = 2,
        Error = 4
    }
}