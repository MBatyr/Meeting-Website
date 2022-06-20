using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public LikesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Список людей которых ты лайкнул
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYourLikePeple()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;            
            var UserLiked = _context.Reciprocity.Include(x => x.PersonBeingLikes)
                                                    .Where(x => x.UserWhoLiked == currentUserId)
                                                    .Select(x => new ReciptoryViewModel
                                                    {
                                                        UserWhoLiked = currentUserId,
                                                        UserBeingLiked = x.UserBeingLiked,
                                                        Path = x.PersonBeingLikes.Path,
                                                        Age = x.PersonBeingLikes.Age,
                                                        FirstName = x.PersonBeingLikes.FirstName,
                                                        LastName = x.PersonBeingLikes.LastName,
                                                        Description = x.PersonBeingLikes.Description
                                                    })
                                                    .ToList();
            if(UserLiked.Count < 1)
            {
                TempData["message"] = "Ты никого не лайкал, лайкни кого нибудь заколебал";
                return RedirectToAction("Search", "Home");
            }
            return View(UserLiked);
        }

        /// <summary>
        /// Удаляет лайк пользователя
        /// </summary>
        /// <param name="reciptory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult>DeleteYourLike(ReciptoryViewModel reciptory)
        {
            if (reciptory == null)
                return NotFound();

            var reciprocityContext = _context.Reciprocity.FirstOrDefault(x => x.UserWhoLiked == reciptory.UserWhoLiked && x.UserBeingLiked == reciptory.UserBeingLiked);
            if (reciprocityContext == null)
                return NotFound();

            _context.Reciprocity.Remove(reciprocityContext);
           await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GetYourLikePeple));
        }

        /// <summary>
        /// Получить список взаимных лайков
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IActionResult GetReciprocity()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var myLikes = _context.Reciprocity.Where(x => x.UserWhoLiked == currentUserId)
                                                .Select(x => new FullReciptory
                                                {
                                                    UserWhoLiked = x.UserWhoLiked,
                                                    UserBeingLiked = x.UserBeingLiked
                                                });
            var whoMeLikes = _context.Reciprocity.Where(x => x.UserBeingLiked == currentUserId)
                                                    .Select(x => new FullReciptory
                                                    {
                                                        UserWhoLiked = x.UserBeingLiked,
                                                        UserBeingLiked = x.UserWhoLiked
                                                    });
            var result = myLikes.Intersect(whoMeLikes).ToList();

            if(result.Count < 1)
            {
                TempData["message"] = "Ты очень не красивый тебя покачто никто не лайкнул";
                return RedirectToAction("Search","Home");
            }

            var fullInfoUser = new List<FullReciptory>();
            FullReciptory oneInfoUser = new FullReciptory();
            var oneReciptor = new Reciprocity();
            for (int i = 0; i < result.Count; i++)
            {
                oneReciptor = _context.Reciprocity.Include(x => x.PersonBeingLikes).FirstOrDefault(x => x.UserWhoLiked == result[i].UserWhoLiked);
                if (oneReciptor != null)
                {
                    oneInfoUser.UserWhoLiked = currentUserId;
                    oneInfoUser.UserBeingLiked = oneReciptor.UserBeingLiked;
                    oneInfoUser.FirstName = oneReciptor.PersonBeingLikes.FirstName;
                    oneInfoUser.LastName = oneReciptor.PersonBeingLikes.LastName;
                    oneInfoUser.Path = oneReciptor.PersonBeingLikes.Path;
                    fullInfoUser.Add(oneInfoUser);
                }
            }
            return View(fullInfoUser);
        }

        /// <summary>
        /// Для получения инфы о пользоветеле которого выбрал текущий пользователь
        /// </summary>
        /// <param name="currentUserEmail"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public async Task <IActionResult> GetDeatails(string currentUserId, string userBeingLiked)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userBeingLiked);

            if (user == null)
                return NotFound();

            var userFullInfo = new FullUserInfoViewModel
            {
                UserWhoLiked = currentUserId,
                UserBeingLiked = userBeingLiked,
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


        /// <summary>
        /// Для рандомног вывода пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <param name="idUserSkip"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetRandomUser(string idUserSkip)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIds = _context.Reciprocity.Where(x => x.UserWhoLiked == currentUserId).Select(x => x.UserBeingLiked).Distinct().ToList();
            userIds.Add(currentUserId);
            if (idUserSkip != null)
                userIds.Add(idUserSkip);

            var users = await _context.Users
                .Where(x => !userIds.Contains(x.Id))
                .Select(x => new NotFullUserInfoViewModel
                {
                    UserBeingLiked = x.Id,
                    Path = x.Path,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Description = x.Description,
                    Age = x.Age
                })
                .ToListAsync();
            if(users.Count < 1)
            {
                TempData["message"] = "Ты уже всех пролайкал";
                return RedirectToAction("GetYourLikePeple","Likes");
            }

            Random rnd = new Random();

            var user = users[rnd.Next(0, users.Count/* + 1*/)];
            return View(new NotFullUserInfoViewModel
            {
                UserWhoLiked = currentUserId,
                UserBeingLiked = user.UserBeingLiked,
                Path = user.Path,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Description = user.Description,
                Age = user.Age
            });
        }


        /// <summary>
        /// Добавляет лайк в бд 
        /// </summary>
        /// <param name="reciptory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddReciptory(FullReciptory reciptory)
        {
            if (reciptory.UserBeingLiked == null || reciptory.UserWhoLiked == null)
                return NotFound();

            _context.Reciprocity.Add(new Reciprocity { UserWhoLiked = reciptory.UserWhoLiked,UserBeingLiked =reciptory.UserBeingLiked});
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GetRandomUser),new { id  = reciptory.UserWhoLiked});
        }
    }
}
    