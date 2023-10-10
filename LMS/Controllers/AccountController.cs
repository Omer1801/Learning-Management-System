using LMS.Data;
using LMS.Models;
using LMS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.Username);

            if(user != null)
            {
                //user is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password,false/*TODO change*/,false);
                    if (result.Succeeded) 
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //wrong password
                TempData["Error"] = "Wrong Credentials. Please try again.";
                return View(loginViewModel);
            }
            //User not found
            TempData["Error"] = "Wrong Credentials. Please try again.";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid) return View(registerViewModel);

            
            if(await _userManager.FindByNameAsync(registerViewModel.Username) != null)
            {
                TempData["Error"] = "Username exists.";
                return View(registerViewModel);
            }
            else if (await _userManager.FindByEmailAsync(registerViewModel.EmailAddress) != null)
            {
                TempData["Error"] = "Email exists.";
                return View(registerViewModel);
            }
            else
            {
                var newUser = new User()
                {
                    Email = registerViewModel.EmailAddress,
                    UserName = registerViewModel.Username,
                    Role = registerViewModel.Role
                };
                var newUserResponse = await _userManager.CreateAsync(newUser,registerViewModel.Password);

                if(newUserResponse.Succeeded)
                    await _userManager.AddToRoleAsync(newUser,registerViewModel.Role);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
