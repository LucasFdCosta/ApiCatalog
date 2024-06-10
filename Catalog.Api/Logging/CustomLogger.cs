using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomLogger(CustomLoggerProviderConfiguration config, string name)
        {
            loggerConfig = config;
            loggerName = name;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} = {formatter(state, exception)}";

            WriteTextOnFile(message);
        }

        private void WriteTextOnFile(string message)
        {
            string filePath = @"C:\MyProjects\ApiCatalog\Catalog.Api\Logging\test_log.txt";

            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}