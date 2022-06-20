using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.Models;
using Сайт_Знакомств.ViewModels;
using Сайт_Знакомств.ViewModels.UserViewModels;

namespace Сайт_Знакомств.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string email,string nams)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                return NotFound();

            var rec = _context.Reciprocity.Include(x => x.User2).Where(x => x.User1.Id == currentUserId).ToList();

            var users = _context.Users
                .Where(x => x.Sex != currenUser.Sex)
                      .Select(x => new NotFullUserInfoViewModel
                      {
                          User1Id = currentUserId,
                          User2Id = x.Id,
                          FirstName = x.FirstName,
                          LastName = x.LastName,
                          Path = x.Path,
                          Age = x.Age,
                      }).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < rec.Count; j++)
                {
                    if (users[i].User2Id == rec[j].User2Id)
                    {
                        users.Remove(users[i]);
                    }
                }
            }
            return View(users);
        }

        public IActionResult UserFilter(UserFilterViewModel  model)
        {
            if (model.Sex == 1)
            {

                var pers = _context.Users.ToList()
                .Where(x => x.Age >= model.MinAge)
                .Where(x => x.Age <= model.MaxAge)
                .Where(x => x.Sex == Enums.Sex.Мужчина)
              .Select(x => new NotFullUserInfoViewModel
              {
                  User2Id = x.Id,
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
               .Where(x => x.Age >= model.MinAge)
               .Where(x => x.Age <= model.MaxAge)
               .Where(x => x.Sex == Enums.Sex.Женщина)
             .Select(x => new NotFullUserInfoViewModel
             {
                 User2Id = x.Id,
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
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var pers = _context.Users
                .Where(x => x.FirstName.Contains(model.UserName))
                .Select(x => new NotFullUserInfoViewModel
                {
                    User1Id = currentUserId,
                    User2Id = x.Id,
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
        [HttpGet]
        public IActionResult Info(string user2Id)
        {
            if (user2Id != null)
            {
                var user = _context.Users.Find(user2Id);
                if (user.Id != null)
                {

                    var model = new NotFullUserInfoViewModel
                    {
                        User2Id = user.Id,
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


        public IActionResult Liked(string user2Id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != null && user2Id != null)
            {
                var user2 = _context.Users.FirstOrDefault(x => x.Id == user2Id);
                if (user2 != null)
                {
                    var connect = new Reciprocity
                    {
                        User1Id = currentUserId,
                        User1Connect = true,
                        User2Id = user2.Id,
                        User2Connect = false
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

