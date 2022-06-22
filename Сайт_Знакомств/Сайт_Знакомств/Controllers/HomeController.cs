using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.Models;
using Сайт_Знакомств.ViewModels;

namespace Сайт_Знакомств.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Search));

                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Делает фильтрацию уже имеющих лайк и выводит всех у кого нет лайка ползователей в базе и передает View()
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Search()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                return NotFound();

            var userIds = _context.Reciprocity.Where(x => x.UserWhoLiked == currentUserId).Select(x => x.UserBeingLiked).Distinct().ToList();
            userIds.Add(currentUserId);

            var users = _context.Users
                      .Where(x => !userIds.Contains(x.Id))
                      .Select(x => new NotFullUserInfoViewModel
                      {
                          UserWhoLiked = currentUserId,
                          UserBeingLiked = x.Id,
                          FirstName = x.FirstName,
                          LastName = x.LastName,
                          Path = x.Path,
                          Age = x.Age,
                      }).ToList();

            if (users.Count < 1)
            {
                TempData["message"] = "Ты уже всех пролайкал, тебе некого показывать";
                return RedirectToAction("GetYourLikePeple", "Likes");
            }
            return View(users);
        }
        
        /// <summary>
        /// филтрация по имени, возраста и пола
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult UserFilter(UserFilterViewModel model)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (model.Sex == 1)
            {

                var pers = _context.Users.ToList()
                .Where(x => x.Id != currentUserId)
                .Where(x => x.Age >= model.MinAge)
                .Where(x => x.Age <= model.MaxAge)
                .Where(x => x.Sex == Enums.Sex.Мужчина)
              .Select(x => new NotFullUserInfoViewModel
              {
                  UserBeingLiked = x.Id,
                  FirstName = x.FirstName,
                  LastName = x.LastName,
                  Path = x.Path,
                  Age = x.Age,

              }).ToList();

                if (pers.Count == 0)
                {
                    ViewBag.Message = "Пользователи такими параметрами не найдены";
                }
                return View(pers);
            }
            else if (model.Sex == 2)
            {
                var pers = _context.Users.ToList()
                .Where(x => x.Id != currentUserId)
               .Where(x => x.Age >= model.MinAge)
               .Where(x => x.Age <= model.MaxAge)
               .Where(x => x.Sex == Enums.Sex.Женщина)
             .Select(x => new NotFullUserInfoViewModel
             {
                 UserBeingLiked = x.Id,
                 FirstName = x.FirstName,
                 LastName = x.LastName,
                 Path = x.Path,
                 Age = x.Age,

             }).ToList();

                if (pers.Count == 0)
                {
                    ViewBag.Message = "Пользователи такими параметрами не найдены";
                }
                return View(pers);
            }

            else if (model.UserName != null)
            {

                var pers = _context.Users
                .Where(x => x.Id != currentUserId)
                .Where(x => x.FirstName.Contains(model.UserName))
                .Select(x => new NotFullUserInfoViewModel
                {
                    UserBeingLiked = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Path = x.Path,
                    Age = x.Age,
                }).ToList();

                if (pers.Count == 0)
                {
                    return NotFound();
                }

                return PartialView(pers);
            }
            return BadRequest();
        }
        /// <summary>
        /// Выводит информайию о пользователя
        /// </summary>
        /// <param name="userBeingLiked"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Info(string userBeingLiked)
        {
            if (userBeingLiked != null)
            {
                var user = _context.Users.Find(userBeingLiked);
                if (user.Id != null)
                {

                    var model = new NotFullUserInfoViewModel
                    {
                        UserBeingLiked = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Description = user.Description,
                        Path = user.Path,
                        Age = user.Age
                    };

                    return View(model);
                }
                return NotFound();
            }
            return NotFound();
        }

        /// <summary>
        /// Сохраняет в базу пользователя получивщий лайк
        /// </summary>
        /// <param name="userBeingLiked"></param>
        /// <returns></returns>
        public IActionResult Liked(string userBeingLiked)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null && userBeingLiked != null)
            {
                var user2 = _context.Users.FirstOrDefault(x => x.Id == userBeingLiked);
                if (user2 != null)
                {
                    var connect = new Reciprocity
                    {
                        UserWhoLiked = currentUserId,
                        UserBeingLiked = user2.Id,
                    };
                    _context.Reciprocity.Add(connect);
                    _context.SaveChanges();
                    return RedirectToAction("Search", "Home");
                }
                return NotFound();
            }
            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

