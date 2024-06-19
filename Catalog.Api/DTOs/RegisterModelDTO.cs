using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.DTOs
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}