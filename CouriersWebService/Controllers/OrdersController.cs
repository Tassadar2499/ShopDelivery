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
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }
			Console.WriteLine("Recieved order");
			await _manager.HandleOrderAsync(order);
		}
	}
}