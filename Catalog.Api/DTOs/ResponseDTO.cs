using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.DTOs
{
    public class ResponseDTO
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}