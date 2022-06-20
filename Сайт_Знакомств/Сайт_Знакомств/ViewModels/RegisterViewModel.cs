using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Сайт_Знакомств.Enums;

namespace Сайт_Знакомств.ViewModels
{

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Обязательно укажите почту")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Не верный формат почты")]
        public string Email { get; set; }
       

        [Required(ErrorMessage = "Ты куда без пароля")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Я знаю ты заколебался но заполни и это")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Обязательно укажите дату рождения")]
        [Display(Name = "Год рождения")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public IFormFile Avatar { get; set; }

        [Required]
        public Sex Sex { get; set; }
        
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Description { get; set; } 
    }

}
