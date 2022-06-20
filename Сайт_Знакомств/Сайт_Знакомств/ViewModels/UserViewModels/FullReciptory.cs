using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Сайт_Знакомств.ViewModels.UserViewModels
{
    public class FullReciptory
    {
        public string UserWhoLiked { get; set; }
        public string UserBeingLiked { get; set; }

        [Display(Name ="Имя")]
        public string FirstName { get; set; }
        [Display(Name ="Фамилие")]
        public string LastName { get; set; }
        public string Path { get; set; }
    }
}
