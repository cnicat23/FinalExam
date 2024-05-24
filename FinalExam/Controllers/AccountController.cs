using FinalExam.Core.Models;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterVm memberRegisterVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = null;

            user = await _userManager.FindByNameAsync(memberRegisterVm.UserName);
            if (user != null)
            {
                ModelState.AddModelError("UserName", "UserName already exist");
                return View();
            }
            user = await _userManager.FindByEmailAsync(memberRegisterVm.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Email already exist");
                return View();
            }

            user = new AppUser()
            {
                FullName = memberRegisterVm.UserName,
                UserName = memberRegisterVm.UserName,
                Email = memberRegisterVm.Email,
            };

            await _userManager.CreateAsync(user, memberRegisterVm.Password);
            await _userManager.AddToRoleAsync(user, "Member");

            return RedirectToAction("Login");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginVm memberLoginVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.FindByNameAsync(memberLoginVm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "UserName or Password is not valid");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, memberLoginVm.Password, memberLoginVm.IsPersistent, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is not valid");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
