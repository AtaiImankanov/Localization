
using lavAspMvclast.Models;
using lavAspMvclast.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LabInsta.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)

        {

            _userManager = userManager;

            _signInManager = signInManager;

        }

        
        [HttpGet]

        public IActionResult Register()

        {
           

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel model)

        {

            if (ModelState.IsValid)

            {

                User user = new User
                {
                               Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                

                //Создание пользователя средствами Identity на основе объекта пользователя и его пароля

                //Возвращает результат выполнения в случае успеха впускаем пользователя в систему

                if (result.Succeeded)

                {
                
                    await _userManager.AddToRoleAsync(user, "user");
                    await _signInManager.SignInAsync(user, false);
                    

                    return RedirectToAction("Index", "ToDoTasks");

                }

                foreach (var error in result.Errors)

                    ModelState.AddModelError(string.Empty, error.Description);

            }

            return View(model);
 
        }
        [HttpGet]

        public IActionResult Login(string returnUrl = null)

        {

            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)

        {

            if (ModelState.IsValid)

            {

                User user = await _userManager.FindByEmailAsync(model.Email);
                if(user== null)
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return View(model);
                }
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(

                    user,

                    model.Password,

                    model.RememberMe,

                    false

                    );

                if (result.Succeeded)

                {

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))

                    {

                        return Redirect(model.ReturnUrl);

                    }



                    return RedirectToAction("Index", "ToDoTasks");

                }

                ModelState.AddModelError("", "Неправильный логин и (или) пароль");

            }

            return View(model);

        }



        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> LogOff()

        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "ToDoTasks");

        }
    }
}
