using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public LogLevel LogLevel { get; set; }
        public int EventId { get; set; }
    }
}