using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сайт_Знакомств.Interface
{
    interface IEntity<T>
    {
        T Id { get; set; }
    }
}
