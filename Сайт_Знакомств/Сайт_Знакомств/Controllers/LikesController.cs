using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.Models;

namespace Сайт_Знакомств.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public LikesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Список людей которых ты лайкнул
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //[HttpGet]
        public IActionResult GetYourLikePeple(string email)
        {
            var user = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(x => x.Email == email);

            if (currentUser == null)
                return NotFound();

            var UserLiked = _context.Reciprocity.Include(x => x.User2).Where(x => x.User1Id == currentUser.Id ).ToList();

            return View(UserLiked);
        }

       
        public IActionResult DeleteYourLike(int id)
        {
           var reciprocity = _context.Reciprocity.Include(x => x.User1).FirstOrDefault(x => x.Id == id);
            if (reciprocity == null)
                return NotFound();
            _context.Reciprocity.Remove(reciprocity);
            _context.SaveChanges();
            return RedirectToAction(nameof(GetYourLikePeple), new { email = reciprocity.User1.Email});
        }


        /// <summary>
        /// Посмотреть список людей которым ты понравился
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListPeopleLikedYou(string email)
        {
            var user = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(x => x.Email == email);
            if (currentUser == null)
                return NotFound();
            var UserLiked = _context.Reciprocity.Include(x => x.User1).Where(x => x.User2Id == currentUser.Id && x.User2Connect == false).ToList();
            return View(UserLiked);
        }

        /// <summary>
        /// Нужен чтоб полььзователь мог ответить взаимностью
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Reciprocate(int id)
        {
            var reciprocity = _context.Reciprocity.Include(x => x.User2).FirstOrDefault(x=> x.Id == id && x.User2Connect== false);
            if(reciprocity != null)
            {
                reciprocity.User2Connect = true;
                _context.Reciprocity.Update(reciprocity);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(GetYourLikePeple), new { email = reciprocity.User2.Email});
        }

        /// <summary>
        /// Получить список взаимных лайков
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //public IActionResult GetReciprocity(string email)
        //{
        //    var currentUser = _context.Users.FirstOrDefault(x => x.Email == email);
        //    if (currentUser == null)
        //        return NotFound();

        //    var listRepociprocitys = _context.Reciprocity.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == currentUser.Id && x.User1Connect == true && x.User2Connect == true
        //                                                                                                        ||
        //                                                                                                        x.User2Id == currentUser.Id && x.User2Connect == true);
            

        //}
    }
}
