
using Logging;

namespace LoggingTests;

public class LoggerBaseTests
{
    internal class TestLogger : LoggerBase
    {
        public TestLogger(Settings settings)
        : base(settings)
        {

        }

        public string WriteLog(LogLevel level, string subject, string message)
        {
            string output = string.Empty;

            if(IsSameLevel(level))
            {
                output = FormatMessage(level, subject, message);
            }

            return output;
        }
    }

    [TestCase(LogLevel.Info, LogLevel.Info, false, TestName="Logged when both log levels are same")]
    [TestCase(LogLevel.Warning, LogLevel.Info, true, TestName="Not logged when both log levels are different")]
    [TestCase(LogLevel.Warning | LogLevel.Error, LogLevel.Error, false, TestName="Logged when both settings flags contain write log level")]
    public void LogLevelOutputTests(
        LogLevel settingsLevel, 
        LogLevel writeLevel,
        bool expectedEmptyStringResult)
    {
        Settings settings = new Settings()
        {
            LevelsToReport = settingsLevel
        };

        TestLogger logger = new TestLogger(settings);
        string result = logger.WriteLog(writeLevel, "Subject", "Message");

        Assert.That(string.IsNullOrEmpty(result), Is.EqualTo(expectedEmptyStringResult));
    }

    [Test]
    public void FormatMessageText()
    {
        Settings settings = new Settings()
        {
            LevelsToReport = LogLevel.Info
        };

        TestLogger logger = new TestLogger(settings);
        string result = logger.WriteLog(LogLevel.Info, "Subject", "Message");
        
        Assert.That(result, Does.Contain("Subject"));
        Assert.That(result, Does.Contain("Message"));
    }
}