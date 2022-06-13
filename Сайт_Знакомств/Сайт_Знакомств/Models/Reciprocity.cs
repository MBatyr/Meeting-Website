using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Interface;

namespace Сайт_Знакомств.Models
{
    public class Reciprocity: IEntity<int>
    {
        
        public int Id { get; set; }


        /// <summary>
        /// Человек который поставил нравится/like
        /// </summary>
        [ForeignKey(nameof(User1))]
        public string User1Id { get; set; }


        /// <summary>
        /// Человек которого лайкнули
        /// </summary>
        [ForeignKey(nameof(User2 ))]
        public string User2Id { get; set; }

        public bool User1Connect { get; set; } = true;
        public bool User2Connect { get; set; } = false;

        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }

    }
}
