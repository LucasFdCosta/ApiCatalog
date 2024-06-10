using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration loggerConfig;
        readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(loggerConfig, name));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}