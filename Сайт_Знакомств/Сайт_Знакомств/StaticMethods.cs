using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.Enums;
using Сайт_Знакомств.Models;

namespace Сайт_Знакомств
{
    /// <summary>
    /// Этот класс создан для хранения различных статичныхМетодов
    /// </summary>
    public static class StaticMethods
    {
        /// <summary>
        /// Добавляет в метод дефолтных пользователей
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static async Task InitialContextAsync(this ApplicationDbContext _context, UserManager<User> userManager)
        {
            if (!_context.Users.Any())
            {
                string password = "12345";
                var user1 = new User
                {
                    Email = "Baku@gmail.com",
                    UserName = "Baku@gmail.com",
                    FirstName = "Baktybek",
                    LastName = "Zhumabekov",
                    Path = "/Files/Настолки.jpg",
                    Description = "Привет меня зовут Бактыбек! Я учусь на программиста буду рад познакомиться",
                    DateOfBirth = new DateTime(2003,02,06),
                    Sex = Sex.Мужчина,
                    PhoneNumber = "+996555818658",
                };
                var user2 = new User
                {
                    Email = "Alim@gmail.com",
                    UserName = "Alim@gmail.com",
                    FirstName = "ALim",
                    LastName = "Bogomolets",
                    Path = "/Files/Алим3.png",
                    Description = "Привет меня зовут Алим! Я очень классный прогер, знакомлюсь не со всеми",
                    DateOfBirth = new DateTime(2000, 02, 06),
                    Sex = Sex.Мужчина,
                    PhoneNumber = "+99655545454545",
                };

                var user3 = new User
                {
                    Email = "Bokser@gmail.com",
                    UserName = "Bokser@gmail.com",
                    FirstName = "Bokser",
                    LastName = "Bokserovich",
                    Path = "/Files/Алим2.png",
                    Description = "Я элитный боксер!Ищу горячую девушку",
                    DateOfBirth = new DateTime(2001, 02, 06),
                    Sex = Sex.Мужчина,
                    PhoneNumber = "+9965507929845",
                };

                var user4 = new User
                {
                    Email = "Buhach@gmail.com",
                    UserName = "Buhach@gmail.com",
                    FirstName = "Buhach",
                    LastName = "Buhachovich",
                    Path = "/Files/Алим1.png",
                    Description = "Люблю хорошенько выпить! Не люблю когда девушка пьет больше меня",
                    DateOfBirth = new DateTime(1995, 02, 06),
                    Sex = Sex.Мужчина,
                    PhoneNumber = "+996779427772",
                };

                var user5 = new User
                {
                    Email = "Halk@gmail.com",
                    UserName = "Halk@gmail.com",
                    FirstName = "Halk",
                    LastName = "Greenovich",
                    Path = "/Files/halk-1.webp",
                    Description = "Халк любить крушить,любить кушать, любить девушек",
                    DateOfBirth = new DateTime(1994, 02, 06),
                    Sex = Sex.Мужчина,
                    PhoneNumber = "+996779427772",
                };

                var user6 = new User
                {
                    Email = "Masha@gmail.com",
                    UserName = "Masha@gmail.com",
                    FirstName = "Masha",
                    LastName = "Mashova",
                    Path = "/Files/Маша.jpg",
                    Description = "Ищу себе нового мишку,плиз найдись",
                    DateOfBirth = new DateTime(2000, 02, 06),
                    Sex = Sex.Женщина,
                    PhoneNumber = "+99670755154898",
                };

                var user7 = new User
                {
                    Email = "Joli@gmail.com",
                    UserName = "Joli@gmail.com",
                    FirstName = "Anjelina",
                    LastName = "Joli",
                    Path = "/Files/Anjelina.jfif",
                    Description = "Ищу замену Бред Питу.Буду рада любому",
                    DateOfBirth = new DateTime(1998, 02, 06),
                    Sex = Sex.Женщина,
                    PhoneNumber = "+99670755154898",
                };

                var user8 = new User
                {
                    Email = "Maria@gmail.com",
                    UserName = "Maria@gmail.com",
                    FirstName = "Maria",
                    LastName = "Anne",
                    Path = "/Files/anne-marie.jpg",
                    Description = "Хорошая девушка,никого не обижаю,люблю выпить",
                    DateOfBirth = new DateTime(1990, 02, 06),
                    Sex = Sex.Женщина,
                    PhoneNumber = "+99670755154898",
                };

                var user9 = new User
                {
                    Email = "Klark@gmail.com",
                    UserName = "Klark@gmail.com",
                    FirstName = "Klark",
                    LastName = "Emilia",
                    Path = "/Files/Emilia.jpg",
                    Description = "Не люблю пить,гулять до поздна.Хочу хорошего солидного мужика",
                    DateOfBirth = new DateTime(1980, 02, 06),
                    Sex = Sex.Женщина,
                    PhoneNumber = "+99670755154898",
                };
                var user10 = new User
                {
                    Email = "Emma@gmail.com",
                    UserName = "Emma@gmail.com",
                    FirstName = "Emma",
                    LastName = "Uotson",
                    Path = "/Files/Emma.jpeg",
                    Description = "Рыжая веселая девушка, хочу рыжих детей,засматриваюсь на программистов",
                    DateOfBirth = new DateTime(1995, 02, 06),
                    Sex = Sex.Женщина,
                    PhoneNumber = "+99670755154898",
                };

                var user11 = new User
                {
                    Email = "Scarlet@gmail.com",
                    UserName = "Scarlet@gmail.com",
                    FirstName = "Scarlet",
                    LastName = "Johanson",
                    Path = "/Files/scarlett-johansson-high-quality-wallpaper-preview.jpg",
                    Description = "Я Скарлет! Тебе это ничего не говорит",
                    DateOfBirth = new DateTime(1999, 02, 06),
                    Sex = Sex.Женщина,
                    PhoneNumber = "+99670755154898",
                };
              



                await userManager.CreateAsync(user1,password);
                await userManager.CreateAsync(user2,password);
                await userManager.CreateAsync(user3,password);
                await userManager.CreateAsync(user4,password);
                await userManager.CreateAsync(user5,password);
                await userManager.CreateAsync(user6,password);
                await userManager.CreateAsync(user7,password);
                await userManager.CreateAsync(user8,password);
                await userManager.CreateAsync(user9,password);
                await userManager.CreateAsync(user10,password);
                await userManager.CreateAsync(user11,password);
            }
        }

    }
}
