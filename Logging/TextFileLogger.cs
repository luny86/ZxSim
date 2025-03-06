
using System.IO;

namespace Logging
{
    internal class TextFileLogger : LoggerBase, ILogger
    {
        const string TimeStampFormat = "yyyyMMdd";
        public TextFileLogger(Settings settings)
        : base(settings)
        {

        }

        void ILogger.WriteLog(LogLevel level, string subject, string message)
        {
            string path = Path.Join([ Settings.Path, 
                $"log_{DateTime.Now.ToString(TimeStampFormat)}.log"]);

            Console.WriteLine(path);
            if(!Path.IsPathFullyQualified(path))
            {
                throw new FileNotFoundException("Path is not valid for logger settings.");
            }

            using(StreamWriter file = new StreamWriter(path, true))
            {
                file.WriteLine(FormatMessage(level, subject, message));
            }
        }
    }
}