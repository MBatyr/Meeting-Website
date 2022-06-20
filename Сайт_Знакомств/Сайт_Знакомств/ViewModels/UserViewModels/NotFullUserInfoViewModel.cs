using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Enums;

namespace Сайт_Знакомств.ViewModels
{
    public class NotFullUserInfoViewModel
    {
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Path { get; set; }
        public string Description { get; set; } 
        public int Age { get; set; }
    }
}
