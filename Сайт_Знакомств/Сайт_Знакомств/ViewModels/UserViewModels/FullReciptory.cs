using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Сайт_Знакомств.ViewModels.UserViewModels
{
    public class FullReciptory
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }

        [Display(Name ="Имя")]
        public string FirstName { get; set; }
        [Display(Name ="Фамилие")]
        public string LastName { get; set; }
        public string Path { get; set; }
    }
}
