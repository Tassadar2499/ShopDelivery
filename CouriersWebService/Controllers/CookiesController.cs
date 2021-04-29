using CouriersWebService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CookiesController : ControllerBase
	{
		private const string LOGIN_KEY = "LoginKey";

		[HttpGet("SetCookie/{login?}")]
		public void SetCookie([FromQuery] string login)
		{
			HttpContext.Response.Cookies.Append(LOGIN_KEY, login);
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