using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сайт_Знакомств.ViewModels.UserViewModels
{
    public class ReciptoryViewModel
    {
        public string UserWhoLikedEmail { get; set; }
        public string UserBeingLikedEmail { get; set; }

        public int Id { get; set; }
        public string UserWhoLiked { get; set; }
        public string UserBeingLiked { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int Age { get; set; }
    }
}
