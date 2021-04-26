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
			var isSuccess = _authLogic.Login(authData);
			if (isSuccess)
				Cookies.Append(LOGIN_KEY, authData.Login);
		}
	}
}