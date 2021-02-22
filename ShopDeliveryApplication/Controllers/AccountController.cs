using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Entities;
using ShopDeliveryApplication.Models.Logic;
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

			var user = new User { Email = registerData.Email, UserName = registerData.Email };

			var result = await UserManager.CreateAsync(user, registerData.Password);
			if (result.Succeeded)
			{
				var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);

				var urlValues = new { userId = user.Id, code };
				var protocol = HttpContext.Request.Scheme;

				var callbackUrl = Url.Action("ConfirmEmail", "Account", urlValues, protocol);

				var emailService = new EmailService();
				var message = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>";

				await emailService.SendEmailAsync(registerData.Email, "Confirm your account", message);

				return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
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

			var user = await UserManager.FindByNameAsync(loginData.Email);
			if (user != null && !await UserManager.IsEmailConfirmedAsync(user))
			{
				ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");

				return View(loginData);
			}

			var result = await SignInManager.PasswordSignInAsync(loginData.Email, loginData.Password, loginData.RememberMe, false);
			if (result.Succeeded)
				return RedirectToMain();
			else
				ModelState.AddModelError("", "Неправильный логин и (или) пароль");

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

		#region Email

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
				return View("Error");

			var user = await UserManager.FindByIdAsync(userId);
			if (user == null)
				return View("Error");

			var result = await UserManager.ConfirmEmailAsync(user, code);

			return result.Succeeded 
				? RedirectToMain()
				: View("Error");
		}

		#endregion Email

		#region Utils

		private IActionResult RedirectToMain()
			=> RedirectToAction("Index", "Shops");

		#endregion Utils
	}
}