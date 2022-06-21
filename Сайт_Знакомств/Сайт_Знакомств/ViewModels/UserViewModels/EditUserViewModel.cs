using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Сайт_Знакомств.ViewModels.UserViewModels
{
    public class EditUserViewModel
    {
        public string Path { get; set; }
        [Required]
        public IFormFile Avatar { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }       
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

    }
}
