using System.ComponentModel.DataAnnotations.Schema;
using Сайт_Знакомств.Interface;

namespace Сайт_Знакомств.Models
{
    public class Reciprocity: IEntity<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID человека который поставил лайк
        /// </summary>
        [ForeignKey(nameof(PersonWhoLikes))]
        public string UserWhoLiked { get; set; }


        /// <summary>
        /// Id человека которому поставили лайк
        /// </summary>
        [ForeignKey(nameof(PersonBeingLikes ))]
        public string UserBeingLiked { get; set; }


        /// <summary>
        /// связь с пользователем который поставил лайк
        /// </summary>
        public virtual User PersonWhoLikes { get; set; }

        /// <summary>
        /// связь с пользователем которого лайкнули
        /// </summary>
        public  User PersonBeingLikes { get; set; }

    }
}
