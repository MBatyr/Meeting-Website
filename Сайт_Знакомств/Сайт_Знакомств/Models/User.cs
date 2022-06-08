using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Сайт_Знакомств.Enums;

namespace Сайт_Знакомств.Models
{
    public class User : IdentityUser
    {              
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Path { get; set; }
        public int Age
        {
            get
            {
                return (DateTime.Now - DateOfBirth).Days / 365;
            }
        }

        [StringLength(200,MinimumLength =10)]
        public string Description { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }       
        public Sex Sex { get; set; }
    }
}
