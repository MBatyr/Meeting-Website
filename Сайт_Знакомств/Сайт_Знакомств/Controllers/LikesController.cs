using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.Models;
using Сайт_Знакомств.ViewModels;
using Сайт_Знакомств.ViewModels.UserViewModels;

namespace Сайт_Знакомств.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private User _currentUser;
        //private readonly UserManager<User> _userManager;
        public LikesController(ApplicationDbContext context/*, UserManager<User> userManager*/)
        {
            _context = context;
            //_userManager = userManager;
        }
        /// <summary>
        /// Список людей которых ты лайкнул
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYourLikePeple(string email,string id)
        {
            
            if (id != null)
            {
               _currentUser= _context.Users.FirstOrDefault(x => x.Id == id);
            }
            else
            {
               _currentUser= _context.Users.FirstOrDefault(x => x.Email == email);
            }
            if (_currentUser == null)
                return NotFound();
            var UserLiked = _context.Reciprocity.Include(x => x.User2)
                                                    .Where(x => x.User1Id == _currentUser.Id)
                                                    .Select(x => new ReciptoryViewModel
                                                    {
                                                        User1Id = _currentUser.Id,
                                                        User2Id = x.User2Id,
                                                        Path = x.User2.Path,
                                                        Age = x.User2.Age,
                                                        FirstName = x.User2.FirstName,
                                                        LastName = x.User2.LastName,
                                                        Description = x.User2.Description,
                                                        User1Email = _currentUser.Email
                                                    })
                                                    .ToList();
            return View(UserLiked);
        }
        [HttpPost]
        public IActionResult DeleteYourLike(ReciptoryViewModel reciptory)
        {
            if (reciptory == null)
                return NotFound();

            var reciprocityContext = _context.Reciprocity.FirstOrDefault(x => x.User1Id == reciptory.User1Id && x.User2Id == reciptory.User2Id);
            if (reciprocityContext == null)
                return NotFound();

            _context.Reciprocity.Remove(reciprocityContext);
            _context.SaveChanges();
            return RedirectToAction(nameof(GetYourLikePeple), new { id = reciptory.User1Id });
        }
       
        /// <summary>
        /// Получить список взаимных лайков
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IActionResult GetReciprocity(string email, string id)
        {
            if (id != null)
            {
                _currentUser = _context.Users.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                _currentUser = _context.Users.FirstOrDefault(x => x.Email== email);
            }
            if (_currentUser == null)
                return NotFound();
            
            var myLikes = _context.Reciprocity.Where(x => x.User1Id == _currentUser.Id)
                                                .Select(x => new FullReciptory
                                                {
                                                    User1Id = x.User1Id,
                                                    User2Id = x.User2Id
                                                });
            var whoMeLikes = _context.Reciprocity.Where(x => x.User2Id == _currentUser.Id)
                                                    .Select(x => new FullReciptory
                                                    {
                                                        User1Id = x.User2Id,
                                                        User2Id = x.User1Id
                                                    });
            var result = myLikes.Intersect(whoMeLikes).ToList();

            var fullInfoUser = new List<FullReciptory>();
            FullReciptory oneInfoUser  = new FullReciptory();
            var oneReciptor = new Reciprocity();
            for (int i = 0; i < result.Count; i++)
            {
                oneReciptor = _context.Reciprocity.Include(x => x.User2).FirstOrDefault(x => x.User1Id == result[i].User1Id);
                if(oneReciptor != null)
                {               
                    oneInfoUser.User1Id = _currentUser.Id;
                    oneInfoUser.User2Id = oneReciptor.User2Id;
                    oneInfoUser.FirstName = oneReciptor.User2.FirstName;
                    oneInfoUser.LastName = oneReciptor.User2.LastName;
                    oneInfoUser.Path = oneReciptor.User2.Path;
                    fullInfoUser.Add(oneInfoUser);
                }
            }
            return View(fullInfoUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserEmail"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public IActionResult GetDeatails(string currentUserId, string user2Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == user2Id);

            if (user == null)
                return NotFound();

            var userFullInfo = new FullUserInfoViewModel
            {
                User1Id = currentUserId,
                User2Id= user2Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Path = user.Path,
                Description = user.Description,
                Age = user.Age,
                Email = user.Email,
                Phone = user.PhoneNumber
            };
            return View(userFullInfo);
        }

    }
}
