using CouriersWebService.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	public class AuthController : Controller
	{
		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterData registerData)
		{
			throw new NotImplementedException();
		}
	}
}
