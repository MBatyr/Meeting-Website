using System.ComponentModel.DataAnnotations;

namespace Сайт_Знакомств.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Обязательно укажите почту")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Обязательно укажите почту")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }


        public string ReturnUrl { get; set; }
    }
}
