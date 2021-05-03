using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Entities;
using ShopDeliveryApplication.Models.Entities.Authorize;
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
				var callbackUrl = CreateCallbackUrl(user, code, "ConfirmEmail");
				var message = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>";

				var emailService = new EmailService();
				await emailService.SendEmailAsync(registerData.Email, "Confirm your account", message);

				return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

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

		#region Forgot password

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword() => View();

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPasswordData)
		{
			if (!ModelState.IsValid)
				return View(forgotPasswordData);

			var user = await UserManager.FindByEmailAsync(forgotPasswordData.Email);
			if (user == null || !await UserManager.IsEmailConfirmedAsync(user))
				return View("ForgotPasswordConfirmation");

			var code = await UserManager.GeneratePasswordResetTokenAsync(user);
			var callbackUrl = CreateCallbackUrl(user, code, "ResetPassword");
			var message = $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>Сбросить пароль</a>";

			var emailService = new EmailService();
			await emailService.SendEmailAsync(forgotPasswordData.Email, "Reset Password", message);

			return View("ForgotPasswordConfirmation");
		}

		#endregion Forgot password

		#region Reset password

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string code = null)
			=> code == null ? View("Error") : View();

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPassword resetPasswordData)
		{
			if (!ModelState.IsValid)
				return View(resetPasswordData);

			var user = await UserManager.FindByEmailAsync(resetPasswordData.Email);
			if (user == null)
				return View("ResetPasswordConfirmation");

			var result = await UserManager.ResetPasswordAsync(user, resetPasswordData.Code, resetPasswordData.Password);
			if (result.Succeeded)
				return View("ResetPasswordConfirmation");

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(resetPasswordData);
		}

		#endregion Reset password

		#region Utils

		private IActionResult RedirectToMain()
			=> RedirectToAction("Index", "Shops");

		private string CreateCallbackUrl(User user, string code, string localAction)
		{
			var urlValues = new { userId = user.Id, code };
			var protocol = HttpContext.Request.Scheme;

			return Url.Action(localAction, "Account", urlValues, protocol);
		}

		#endregion Utils
	}
}