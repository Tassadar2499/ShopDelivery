using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Entities;
using ShopsDbEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Controllers
{
	public class AccountController : Controller
	{
        private UserManager<User> UserManager { get; }
        private SignInManager<User> SignInManager { get; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
        public async Task<IActionResult> Register(RegisterData registerData)
        {
            if (!ModelState.IsValid)
                return View(registerData);

            var user = new User { Email = registerData.Email, UserName = registerData.Email };
            var result = await UserManager.CreateAsync(user, registerData.Password);

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Shops");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registerData);
        }
    }
}
