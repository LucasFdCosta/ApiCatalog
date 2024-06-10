using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Services
{
    public class MyService : IMyService
    {
        public string Hello(string name)
        {
            return $"Hello {name}!\n\n {DateTime.Today.ToShortDateString()}";
        }
    }
}