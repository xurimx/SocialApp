using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Identity.Entities;
using SocialApp.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialApp.Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IIdentityServerInteractionService interactionService;

        public AccountController(SignInManager<ApplicationUser> signInManager, 
                                 UserManager<ApplicationUser> userManager, 
                                 IIdentityServerInteractionService interactionService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.interactionService = interactionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel login)
        {
            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(login.ReturnUrl);
            }
            return View();
        }

        public async Task<IActionResult> LogoutAsync(string logoutId)
        {
            await signInManager.SignOutAsync();
            var logoutRequest = await interactionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction(nameof(Index), nameof(AccountController));
            }
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel register)
        {
            var newUser = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.Username
            };

            var result = await userManager.CreateAsync(newUser, register.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(newUser, false);
                return Redirect(register.ReturnUrl);
            }

            return View();
        }
    }
}
