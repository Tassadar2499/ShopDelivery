using CouriersWebService.Data;
using CouriersWebService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly CouriersAuthLogic _authLogic;
		private IRequestCookieCollection Cookies => HttpContext.Request.Cookies;
		public AuthController(CouriersAuthLogic authLogic) => _authLogic = authLogic;

		[HttpPost("Register")]
		public async Task RegisterAsync(RegisterData registerData)
		{
			await _authLogic.RegisterAsync(registerData);
		}
	}
}
