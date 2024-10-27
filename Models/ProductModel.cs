using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using StackWebApp.Models;

namespace StackWebApp.Models 
{


    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string? SerialNumber { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        
    }
}