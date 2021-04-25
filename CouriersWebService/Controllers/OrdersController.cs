using CouriersWebService.Services;
using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities.Entities.ProductEntities;
using System;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly OrdersLogic _manager;

		public OrdersController(OrdersLogic manager) => _manager = manager;

		[HttpPost("Handle")]
		public async Task HandleOrderAsync([FromBody] Order order)
		{
			var gg = 0;
			Console.WriteLine("Recieved order");
		}
	}
}