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
        private User _currentUser;
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
        public IActionResult GetYourLikePeple(string email, string id)
        {

            if (id != null)
            {
                _currentUser = _context.Users.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                _currentUser = _context.Users.FirstOrDefault(x => x.Email == email);
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
        public async Task< IActionResult>DeleteYourLike(ReciptoryViewModel reciptory)
        {
            if (reciptory == null)
                return NotFound();

            var reciprocityContext = _context.Reciprocity.FirstOrDefault(x => x.User1Id == reciptory.User1Id && x.User2Id == reciptory.User2Id);
            if (reciprocityContext == null)
                return NotFound();

            _context.Reciprocity.Remove(reciprocityContext);
           await _context.SaveChangesAsync();
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
                _currentUser = _context.Users.FirstOrDefault(x => x.Email == email);
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
            FullReciptory oneInfoUser = new FullReciptory();
            var oneReciptor = new Reciprocity();
            for (int i = 0; i < result.Count; i++)
            {
                oneReciptor = _context.Reciprocity.Include(x => x.User2).FirstOrDefault(x => x.User1Id == result[i].User1Id);
                if (oneReciptor != null)
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
        /// Для получения инфы о пользоветеле которого выбрал текущий пользователь
        /// </summary>
        /// <param name="currentUserEmail"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public async Task <IActionResult> GetDeatails(string currentUserId, string user2Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == user2Id);

            if (user == null)
                return NotFound();

            var userFullInfo = new FullUserInfoViewModel
            {
                User1Id = currentUserId,
                User2Id = user2Id,
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
        public async Task<IActionResult> GetRandomUser(string email, string id, string idUserSkip)
        {
            if (email != null)
                _currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            else
                _currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            var currentUserId = _currentUser.Id;
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIds = _context.Reciprocity.Where(x => x.User1Id == currentUserId).Select(x => x.User2Id).Distinct().ToList();
            userIds.Add(currentUserId);
            if (idUserSkip != null)
                userIds.Add(idUserSkip);

            var users = await _context.Users
                .Where(x => !userIds.Contains(x.Id))
                .Select(x => new NotFullUserInfoViewModel
                {
                    User2Id = x.Id,
                    Path = x.Path,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Description = x.Description,
                    Age = x.Age
                })
                .ToListAsync();

            Random rnd = new Random();

            var user = users[rnd.Next(0, users.Count/* + 1*/)];
            return View(new NotFullUserInfoViewModel
            {
                User1Id = currentUserId,
                User2Id = user.User2Id,
                Path = user.Path,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Description = user.Description,
                Age = user.Age
            });
        }
        [HttpPost]
        public async Task<IActionResult> GetRandomUser(FullReciptory reciptory)
        {
            if (reciptory.User2Id == null || reciptory.User1Id == null)
                return NotFound();

            _context.Reciprocity.Add(new Reciprocity { User1Id = reciptory.User1Id,User2Id =reciptory.User2Id});
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(GetRandomUser),new { id  = reciptory.User1Id});
        }
    }
}
    