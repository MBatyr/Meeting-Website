using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string email)
        {

            var currenUser = _context.Users.FirstOrDefault(x => x.Email == email);
            if (currenUser == null)
                return NotFound();

            var rec = _context.Reciprocity.Include(x => x.User2).Where(x => x.User1.Id == currenUser.Id).ToList();

            var users = _context.Users
                .Where(x => x.Sex != currenUser.Sex)
                      .Select(x => new NotFullUserInfoViewModel
                      {
                          User1Id = currenUser.Id,
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

        [HttpGet]
        public IActionResult UserFilter(string userName1, string userName, int Age1, int Age2, int Sex)
        {

            var user1 = _context.Users.FirstOrDefault(x => x.Email == userName1);
            if (userName != null)
            {

                var pers = _context.Users
                .Where(x => x.FirstName == userName)
                .Select(x => new NotFullUserInfoViewModel
                {
                    //User1Id = user1.Id,
                    User2Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Path = x.Path,
                    Age = x.Age,
                });



                return PartialView(pers.ToList());
            }
            else
            {
                if (Sex == 1)
                {
                    var pers = _context.Users.ToList()
                    .Where(x => x.Age >= Age1)
                    .Where(x => x.Age <= Age2)
                    .Where(x => x.Sex == Enums.Sex.Мужчина)
                  .Select(x => new NotFullUserInfoViewModel
                  {
                      User1Id = user1.Id,
                      User2Id = x.Id,
                      FirstName = x.FirstName,
                      LastName = x.LastName,
                      Path = x.Path,
                      Age = x.Age,

                  }).ToList();
                    return PartialView(pers);
                }
                else
                {
                    var pers = _context.Users.ToList()
                   .Where(x => x.Age >= Age1)
                   .Where(x => x.Age <= Age2)
                   .Where(x => x.Sex == Enums.Sex.Женщина)
                 .Select(x => new NotFullUserInfoViewModel
                 {
                     //User1Id = user1.Id,
                     User2Id = x.Id,
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     Path = x.Path,
                     Age = x.Age,

                 }).ToList();
                    return PartialView(pers);
                }
            }
            return NotFound();
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Info(string user1Id, string user2Id)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(user2Id);
                if (user2Id != null)
                {


                    var model = new NotFullUserInfoViewModel
                    {
                        User1Id = user1Id,
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
        [HttpPost]
        public IActionResult Liked(NotFullUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user1 = _context.Users.FirstOrDefault(x => x.Id == model.User1Id);
                var user2 = _context.Users.FirstOrDefault(x => x.Id == model.User2Id);
                if (user1 != null && user2 != null)
                {
                    var connect = new Reciprocity
                    {
                        User1Id = user1.Id,
                        User1Connect = true,
                        User2Id = user2.Id,
                        User2Connect = false
                    };
                    _context.Reciprocity.Add(connect);
                    _context.SaveChanges();
                    return RedirectToAction("Search", "Home", new { email = user1.Email });
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
