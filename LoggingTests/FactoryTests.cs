
using Logging;

namespace LoggingTest
{
    public class FactoryTests
    {
        [Test]
        public void LoggerIsSameObject()
        {
            Settings settings = new Settings();
            IFactory factory = new Factory(settings);

            ILogger logger1 = factory.GetLogger();
            ILogger logger2 = factory.GetLogger();

            Assert.That(object.ReferenceEquals(logger1, logger2), Is.True,
                "Logger factory is creating a new instance of ILogger each time.");
        }
    }
}