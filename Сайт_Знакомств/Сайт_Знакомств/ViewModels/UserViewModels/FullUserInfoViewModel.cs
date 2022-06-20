using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сайт_Знакомств.ViewModels.UserViewModels
{
    public class FullUserInfoViewModel
    {
        public string UserWhoLiked { get; set; }
        public string UserBeingLiked { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }   
        public string Phone { get; set; }
    }
}
