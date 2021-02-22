using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Entities;
using ShopsDbEntities.Entities;
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

		#region Register

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterData registerData)
		{
			if (!ModelState.IsValid)
				return View(registerData);

			var email = registerData.Email;

			var userExist = await UserManager.FindByEmailAsync(email);
			if (userExist != null)
			{
				ModelState.AddModelError(string.Empty, "Данный пользователь уже зарегистрирован");
				return View(registerData);
			}

			var user = new User
			{
				Email = email,
				UserName = email
			};

			var result = await UserManager.CreateAsync(user, registerData.Password);

			if (result.Succeeded)
			{
				await SignInManager.SignInAsync(user, false);

				return RedirectToMain();
			}
			else
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(registerData);
		}

		#endregion Register

		#region Login

		[HttpGet]
		public IActionResult Login(string returnUrl = null) 
			=> View(new LoginData { ReturnUrl = returnUrl });

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginData loginData)
		{
			if (!ModelState.IsValid)
				return View(loginData);

			var result = await SignInManager.PasswordSignInAsync(loginData.Email, loginData.Password, loginData.RememberMe, false);

			if (result.Succeeded)
			{
				var returnUrl = loginData.ReturnUrl;

				return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
					? Redirect(returnUrl)
					: RedirectToMain();
			}
			else
			{
				ModelState.AddModelError("", "Неправильный логин и (или) пароль");
			}

			return View(loginData);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await SignInManager.SignOutAsync();

			return RedirectToMain();
		}

		#endregion Login

		#region Utils

		private IActionResult RedirectToMain()
			=> RedirectToAction("Index", "Shops");

		#endregion Utils
	}
}