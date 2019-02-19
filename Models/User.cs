

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [MinLength(3)]
        public string FirstName {get; set;}

        [Required]
        [MinLength(3)]
        public string LastName {get; set;}

        [EmailAddress]
        [Required]
        public string Email {get; set;}

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get; set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Wedding> Wedding{get ;set;}
        public List<Guest> Guest{get;set;}

    }
}