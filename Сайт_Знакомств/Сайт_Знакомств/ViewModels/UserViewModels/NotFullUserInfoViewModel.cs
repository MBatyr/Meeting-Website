using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Enums;

namespace Сайт_Знакомств.ViewModels
{
    public class NotFullUserInfoViewModel
    {
        public string UserWhoLiked { get; set; }
        public string UserBeingLiked { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилие")]
        public string LastName { get; set; }
        public string Path { get; set; }

        [Display(Name ="Описание")]
        public string Description { get; set; } 


        [Display(Name ="Возраст")]
        public int Age { get; set; }
    }
}
