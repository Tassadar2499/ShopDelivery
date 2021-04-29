using CouriersWebService.Data;
using CouriersWebService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private const string LOGIN_KEY = "LoginKey";

		private readonly CouriersAuthLogic _authLogic;
		private IResponseCookies Cookies => HttpContext.Response.Cookies;

		public AuthController(CouriersAuthLogic authLogic) => _authLogic = authLogic;

		[HttpGet("Update")]
		public async Task UpdateAsync([FromBody] UpdateCourierData courierData)
		{
			//await _authLogic.UpdateAsync(login);
		}

		[HttpPost("Login")]
		public async Task LoginAsync([FromBody] AuthData authData)
		{
			var isSuccess = await _authLogic.LoginAsync(authData);
			if (isSuccess)
				Cookies.Append(LOGIN_KEY, authData.Login);
		}

		[HttpGet("SetCookie/{login?}")]
		public void SetCookie([FromQuery] string login)
		{
			Cookies.Append(LOGIN_KEY, login);
		}

		[HttpGet("GetCookie")]
		public async Task GetCookieAsync()
		{
			_ = HttpContext.Request.Cookies.TryGetValue(LOGIN_KEY, out var login);
			var cookieData = new CookieData() { Login = login };
			await HttpContext.Response.WriteAsJsonAsync(cookieData);
		}
	}
}