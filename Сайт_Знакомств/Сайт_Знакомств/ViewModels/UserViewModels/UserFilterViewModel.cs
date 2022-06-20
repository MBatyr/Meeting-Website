using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Enums;

namespace Сайт_Знакомств.ViewModels
{
    public class UserFilterViewModel
    {
       public  string UserName { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int Sex { get; set; }
    }
}
