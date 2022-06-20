using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Сайт_Знакомств.Enums;

namespace Сайт_Знакомств.Models
{
    public class User : IdentityUser
    {              
       /// <summary>
       /// Имя поьзователя
       /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилие пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Путь к фотографии
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Свойство чтоб узнать возраст пользователя
        /// </summary>
        public int Age
        {
            get
            {
                return (DateTime.Now - DateOfBirth).Days / 365;
            }
        }

        /// <summary>
        /// Описание пользоваетеля
        /// </summary>
        [StringLength(200,MinimumLength =10)]
        public string Description { get; set; }

        /// <summary>
        /// Хранит в себе дату рождения пользователя
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }       

        /// <summary>
        /// Пол пользователя
        /// </summary>
        public Sex Sex { get; set; }
    }
}
