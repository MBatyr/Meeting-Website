using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Сайт_Знакомств.Data;
using Сайт_Знакомств.ViewModels.UserViewModels;

namespace Сайт_Знакомств.Controllers
{
    public class UserController : Controller
    {        
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ApplicationDbContext _context;
        public UserController(IWebHostEnvironment appEnvironment, ApplicationDbContext context)
        {
            _appEnvironment = appEnvironment;
            _context = context;
        }

        /// <summary>
        /// Дает возможность пользователю посмотреть свои данные
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Profile()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser == null)
                return NotFound();

            var users = _context.Users
                .Where(x => x.Email == currentUserId)
                .Select(x => new FullUserInfoViewModel()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,

                    Path = x.Path,
                    Age = x.Age,
                    Email = x.Email,
                    Phone = x.PhoneNumber
                });
            return View(currentUser);
        }

       
        /// <summary>
        /// Метод для изменения данных пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditProfile()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser == null)
                return NotFound();

            var user = new EditUserViewModel()
            {
                Path = currentUser.Path,
                Avatar = null,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                Description = currentUser.Description,
                DateOfBirth = currentUser.DateOfBirth

            };
            return View(user);
        }


        /// <summary>
        /// Метод Post для изменения данных пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserViewModel model)
        
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(x => x.Id == currentUserId);


            if (model.Avatar != null)
            {
                string path = "/Files/" + model.Avatar.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.Avatar.CopyToAsync(fileStream);
                }

                var currentUser = _context.Users.FirstOrDefault(x => x.Email == model.Email);

                currentUser.Email = model.Email;
                currentUser.UserName = model.Email;
                currentUser.DateOfBirth = model.DateOfBirth;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.PhoneNumber = model.Phone;
                currentUser.Description = model.Description;
                currentUser.Path = path;
            }
            else
            {
                var path = user.Path;

                var currentUser = _context.Users.FirstOrDefault(x => x.Email == model.Email);

                currentUser.Email = model.Email;
                currentUser.UserName = model.Email;
                currentUser.DateOfBirth = model.DateOfBirth;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.PhoneNumber = model.Phone;
                currentUser.Description = model.Description;
                currentUser.Path = path;
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Profile));
        }

    }
}
