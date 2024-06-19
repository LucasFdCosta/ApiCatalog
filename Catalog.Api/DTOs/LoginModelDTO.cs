using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.DTOs
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}