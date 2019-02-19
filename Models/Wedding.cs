using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Wedding 
    {
        [Key]
        public int WeddingId {get; set;}

        [Required]
        [MinLength(4)]
        public string WedderOne {get; set;}

        [Required]
        [MinLength(4)]
        public string WedderTwo{get; set;}

        [Required]
        public DateTime Date{get; set;}

        [Required]
        public string WeddingAdress {get;set;}

        public int UserId {get;set;}
        public User User {get;set;}

        public List<Guest> Guest{get;set;}

    }
}