using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StackWebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool? RememberMe { get; set; }
    }
}