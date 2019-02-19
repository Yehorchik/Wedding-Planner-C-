using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        public string Email {get; set;}
        
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}